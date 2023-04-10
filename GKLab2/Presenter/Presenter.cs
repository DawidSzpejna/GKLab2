using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.Elements;
using GKLab2.Presenter.Algorithm.Structures;
using GKLab2.Presenter.Algorithm.Coloring;
using GKLab2.Presenter.Algorithm.Coloring.NormalVectorModifier;
using GKLab2.Presenter.Algorithm.Coloring.CollorFillers;
using Model.Elements.Interfaces;
using System.Globalization;

namespace GKLab2.Presenter
{
    public class Presenter
    {
        #region Properites
        public Model.GenerelModel MyModel { get; private set; }
        public Pages.MainPage MyPage { get; private set; }
        public FastBitmapLib.FastBitmap FastCanvas { get; private set; }
        public IVFiller iVFiller { get; private set; }
        public INFiller iNFiller { get; private set; }
        public IFiller CurrentFiller { get; set; }

        #region Sun
        public Sun Solaris { get; private set; }
        public float CurrentEngle = 0;
        public const float StepEngle = 10;
        #endregion
        public KSDM Ksdm { get; set; }
        private NumberFormatInfo NumberFormat { get; set; }

        public Matrix4x4 View { get; set; }
        public Matrix4x4 Projection { get; set; }
        public Vector3 PositionCamer;

        public float[,] ZBuffer { get; set; }
        #endregion

        #region Constructors
        public Presenter(Model.GenerelModel model, Pages.MainPage page)
        {
            MyModel = model;
            MyPage = page;
            FastCanvas = new FastBitmapLib.FastBitmap(MyPage.Canvas);

            MyPage.MySliderKS.ValueChanged += new System.EventHandler(this.SliderKS_ValueChanged);
            MyPage.MySliderKD.ValueChanged += new System.EventHandler(this.SliderKD_ValueChanged);
            MyPage.MySliderM.ValueChanged += new System.EventHandler(this.SliderM_ValueChanged);
            MyPage.MySliderH.ValueChanged += new System.EventHandler(this.SliderH_ValueChanged);
            MyPage.MySliderV.ValueChanged += new System.EventHandler(this.SliderV_ValueChanged);
            MyPage.MySliderZ.ValueChanged += new System.EventHandler(this.SliderZ_ValueChanged);
            MyPage.MySliderFov.ValueChanged += new System.EventHandler(this.SliderFov_ValueChanged);

            MyPage.MyVertexMode.CheckedChanged += new System.EventHandler(this.RadioButtonVrtx_CheckedChanged);
            MyPage.MyVectorMode.CheckedChanged += new System.EventHandler(this.RadioButtonVctr_CheckedChanged);
            MyPage.MyCheckBoxGrid.Click += new System.EventHandler(this.RadioButtonGrid_Click);
            MyPage.MySellectLightColorBt.Click += new System.EventHandler(this.SellectLightColorBt_Click);
            MyPage.MySellectObjectColorBt.Click += new System.EventHandler(this.SellectObjectColorBt_Click);
            MyPage.MyMovingShapsBt.Click += new System.EventHandler(this.MyMovingShapsBt_Click);
            MyPage.MyLoadTextureBt.Click += new System.EventHandler(this.LoadTextureBt_Click);
            MyPage.MyLoadNormalMapBt.Click += new System.EventHandler(this.LoadNormalMapBt_Click);
            MyPage.MyLoadShapeBt.Click += new System.EventHandler(this.LoadShapeBt_Click);
            MyPage.MyStartSunAnimationBt.Click += new System.EventHandler(this.StartSunAnimationBt_Click);
            MyPage.MyLoadNormalMapCheckB.Click += new System.EventHandler(this.LoadNormalMapCheckB_Click);
            MyPage.MySolarTimer.Tick += new System.EventHandler(this.TimerSolarAnimation_Tick);
            MyPage.ThisTimer.Tick += new System.EventHandler(this.timer1_Tick);
            MyPage.MForX.ValueChanged += new System.EventHandler(this.ForX_ValueChanged);
            MyPage.MForY.ValueChanged += new System.EventHandler(this.ForY_ValueChanged);
            MyPage.MForZ.ValueChanged += new System.EventHandler(this.ForZ_ValueChanged);

            Solaris = new Sun(  MyPage.MySliderH.Value,
                                MyPage.MySliderV.Value,
                                MyPage.MySliderZ.Value,
                                MyPage.MySliderH.Value / MyModel.Size,
                                MyPage.MySliderV.Value / MyModel.Size,
                                MyPage.MySliderZ.Value / MyModel.Size, new Vector3(1, 1, 1));

            Ksdm = new KSDM(MyPage.MySliderKS.Value / MyModel.Size, 
                    MyPage.MySliderKD.Value / MyModel.Size, 
                    MyPage.MySliderM.Value);

            iVFiller = new IVFiller(Ksdm);
            iNFiller = new INFiller(Ksdm);

            SetDefaultSettingOfModel();
            CurrentFiller = iNFiller;
            //CurrentFiller = new ConstFiller(Ksdm);

            // Initialize default value for the object
            MyModel.MyShapes[0].CurrentModifier = new ConstNormalVector();
            MyModel.MyShapes[0].CurrentColor = new ColorFillerConst(new Vector3(0.5f, 0.5f, 1));

            NumberFormat = new NumberFormatInfo();
            NumberFormat.NumberDecimalDigits = 2;


            View = Matrix4x4.Identity;
            PositionCamer = new Vector3(1000f, 0, 0);
            View = Matrix4x4.CreateLookAt(PositionCamer, new Vector3(0, 0, 1), new Vector3(0, 0, 1));

            Projection = Matrix4x4.Identity;
            Projection = Matrix4x4.CreatePerspectiveFieldOfView(30f / 120f,
                                                                (float)MyPage.CanvasWidht / (float)MyPage.CanvasHeight,
                                                                1000,
                                                                5000
                                                                );

            ZBuffer = new float[MyPage.CanvasHeight, MyPage.CanvasWidht];

            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }
        #endregion

        #region Drawing
        public void DrawAllShapes(Bitmap map, List<Shape> shapes)
        {
            for (int i = 0; i < MyPage.CanvasHeight; i++)
            {
                for (int j = 0; j < MyPage.CanvasWidht; j++)
                {
                    ZBuffer[i, j] = float.PositiveInfinity;
                }
            }

            foreach (var shp in shapes)
            {
                DrawShapeInCenter(map, shp);
            }
        }


        public void DrawShapeInCenter(Bitmap map, Model.Elements.Shape shp)
        {
            //float minSize = MyPage.CanvasWidht;
            //if (minSize > MyPage.CanvasHeight)
            //{
            //    minSize = MyPage.CanvasHeight;
            //}
            //minSize *= 0.5f;
            
            //FillAll(shp);
            
            {
                using (Graphics g = Graphics.FromImage(map))
                {
                    foreach (Triangle tr in shp.Triangles)
                    {
                        MPoint Am = shp.PipeOfView(View, Projection, tr.P1, map.Size.Width, map.Size.Height);
                        MPoint Bm = shp.PipeOfView(View, Projection, tr.P2, map.Size.Width, map.Size.Height);
                        MPoint Cm = shp.PipeOfView(View, Projection, tr.P3, map.Size.Width, map.Size.Height);

                        Triangle tmp = new Triangle(new Edge(Am, Bm), new Edge(Am, Cm), new Edge(Bm, Cm), Am, Bm, Cm);
                        CurrentFiller.Init(tmp, Solaris, shp.Center, shp.CurrentModifier, shp.CurrentColor);
                        Algorithm.Algs.FillPolygonAlg(this, tmp, shp.Center);

                        if (MyPage.MyCheckBoxGrid.Checked == true)
                        {
                            g.DrawLine(Pens.Black, ConvertX(Am.X, shp.Center), ConvertY(Am.Y, shp.Center), ConvertX(Bm.X, shp.Center), ConvertY(Bm.Y, shp.Center));
                            g.DrawLine(Pens.Black, ConvertX(Am.X, shp.Center), ConvertY(Am.Y, shp.Center), ConvertX(Cm.X, shp.Center), ConvertY(Cm.Y, shp.Center));
                            g.DrawLine(Pens.Black, ConvertX(Bm.X, shp.Center), ConvertY(Bm.Y, shp.Center), ConvertX(Cm.X, shp.Center), ConvertY(Cm.Y, shp.Center));
                        }
                    }


                    //foreach (Model.Elements.Edge edge in shp.Edges)
                    //{
                    //    MPoint Am = shp.PipeOfView(View, Projection, edge.A, map.Size.Width, map.Size.Height);
                    //    MPoint Bm = shp.PipeOfView(View, Projection, edge.B, map.Size.Width, map.Size.Height);

                    //    g.DrawLine(Pens.Black, ConvertX(Am.X, shp.Center), ConvertY(Am.Y, shp.Center), ConvertX(Bm.X, shp.Center), ConvertY(Bm.Y, shp.Center));
                    //}
                }
            }

            //MyPage.DARefresh();
        }

        public void FillAll(Shape shp)
        {
            foreach(Triangle tr in shp.Triangles)
            {
                CurrentFiller.Init(tr, Solaris, shp.Center, shp.CurrentModifier, shp.CurrentColor);
                Algorithm.Algs.FillPolygonAlg(this, tr, shp.Center);
            }
        }

        public void DrawLines(Triangle tr, MPoint center, List<AETnode> list, int y)
        {
            int count = list.Count / 2;
            for (int i = 0; i < count; i++)
            {
                DrawLine(tr, center, list[i * 2].Xcurrent, list[i * 2 + 1].Xcurrent, y);
            }
        }

        public void DrawLine(Triangle tr, MPoint center, float x1, float x2, int y)
        {
            float xMax = x1;
            float xMin = x2;
            if (x2 > x1)
            {
                xMax = x2;
                xMin = x1;
            }

            xMax = Round(xMax);
            xMin = Round(xMin);

            FastCanvas.Lock();
            for (float x = xMin; x <= xMax; x++)
            {
                Color cl = CurrentFiller.ProperColour(x, y);

                int _x = Round(ConvertX(x, center));
                int _y = Round(ConvertY(y, center));
                if (_x < 0 || _x >= MyPage.CanvasWidht || _y < 0 || _y >= MyPage.CanvasHeight) continue;

                // LABY
                MPoint tmp = new MPoint(x, y);
                (float u, float v, float w) = BasicFiller.GiveMeUVW_withoutZ(tr, tmp);
                Vector3 tmp_v = BasicFiller.BarycentricInterpolation(tr.P1, tr.P2, tr.P3, u, v, w);

                if (tmp_v.Z <= ZBuffer[_y, _x])
                {
                    ZBuffer[_y, _x] = tmp_v.Z;
                    FastCanvas.SetPixel(_x, _y, cl);
                }
                // ---- END



            }
            FastCanvas.Unlock();
        }
        #endregion

        #region EventHandlers
        #region Sliders
        private void SliderKS_ValueChanged(object sender, EventArgs e)
        {
            TrackBar trackB = sender as TrackBar;
            Ksdm.ks = trackB.Value / 100f;
            MyPage.MyKsLabel.Text = Ksdm.ks.ToString("N", NumberFormat);


            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }

        private void SliderKD_ValueChanged(object sender, EventArgs e)
        {
            TrackBar trackB = sender as TrackBar;
            Ksdm.kd = trackB.Value / 100f;
            MyPage.MyKdLabel.Text = Ksdm.kd.ToString("N", NumberFormat);

            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }

        private void SliderM_ValueChanged(object sender, EventArgs e)
        {
            TrackBar trackB = sender as TrackBar;
            Ksdm.m = trackB.Value;
            MyPage.MyMLabel.Text = Ksdm.m.ToString("N", NumberFormat);

            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }

        private void SliderH_ValueChanged(object sender, EventArgs e)
        {
            TrackBar trackB = sender as TrackBar;
            CountSunX(trackB.Value);

            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }

        private void SliderV_ValueChanged(object sender, EventArgs e)
        {
            TrackBar trackB = sender as TrackBar;
            CountSunY(trackB.Value);

            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }

        private void SliderZ_ValueChanged(object sender, EventArgs e)
        {
            TrackBar trackB = sender as TrackBar;
            CountSunZ(trackB.Value);

            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }

        private void SliderFov_ValueChanged(object sender, EventArgs e)
        {
            TrackBar trackB = sender as TrackBar;

            float fov = trackB.Value;
            Projection = Matrix4x4.CreatePerspectiveFieldOfView(fov / 120f,
                                                                (float)MyPage.CanvasWidht / (float)MyPage.CanvasHeight,
                                                                1000,
                                                                5000
                                                                );

            MyPage.Clera();
            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }
        #endregion

        #region RadioButtons
        private void RadioButtonVrtx_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton but = sender as RadioButton;
            if (but.Checked)
            {
                CurrentFiller = iVFiller;
                DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
                MyPage.DARefresh();
            }
        }

        private void RadioButtonVctr_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton but = sender as RadioButton;
            if (but.Checked)
            {
                CurrentFiller = iNFiller;
                DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
                MyPage.DARefresh();
            }
        }

        private void LoadNormalMapCheckB_Click(object sender, EventArgs e)
        {
            MyPage.MyLoadNormalMapCheckB.Checked = false;
            MyPage.MyLoadNormalMapCheckB.Enabled = false;
            //MyModel.MyShape.CurrentModifier = new ConstNormalVector();

            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }

        private void RadioButtonGrid_Click(object sender, EventArgs e)
        {
            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }
        #endregion

        #region Buttons
        private void SellectObjectColorBt_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    //MyModel.MyShape.CurrentColor = new ColorFillerConst(colorDialog.Color);
                }

                DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
                MyPage.DARefresh();
            }
        }

        private void SellectLightColorBt_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Solaris.SetColor(colorDialog.Color);
                }

                DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
                MyPage.DARefresh();
            }

        }

        private void LoadTextureBt_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\Textures";
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }

            if (filePath != string.Empty)
            {
                Bitmap bmp = new Bitmap(filePath);

                if (bmp.Width > MyPage.CanvasWidht && bmp.Height > MyPage.CanvasHeight)
                {
                    bmp = new Bitmap(bmp, new Size(MyPage.CanvasWidht, MyPage.CanvasHeight));
                }
                else if (bmp.Width > MyPage.CanvasWidht)
                {
                    bmp = new Bitmap(bmp, new Size(MyPage.CanvasWidht, bmp.Height));
                }
                else if (bmp.Height > MyPage.CanvasHeight)
                {
                    bmp = new Bitmap(bmp, new Size(bmp.Width, MyPage.CanvasHeight));
                }

                MyModel.MyShapes[0].CurrentColor = new ColorFillerTexture(bmp);
                DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
                MyPage.DARefresh();
            }
        }

        private void LoadNormalMapBt_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\NormalMap";
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }

            if (filePath != string.Empty)
            {
                Bitmap bmp = new Bitmap(filePath);
                bmp = new Bitmap(bmp, new Size(MyPage.CanvasWidht + 5, MyPage.CanvasHeight + 5));

                if (MyPage.MyNRBt.Checked)
                {
                    MyModel.MyShapes[0].CurrentModifier = new ByTextureModifier(bmp);
                }
                else
                {
                    MyModel.MyShapes[0].CurrentModifier = new ByHightMapModifier(bmp);
                }

                DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
                MyPage.DARefresh();
                MyPage.MyLoadNormalMapCheckB.Enabled = true;
                MyPage.MyLoadNormalMapCheckB.Checked = true;
            }
        }

        private void LoadShapeBt_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\DefaultShapes";
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                //openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //MyModel.Reload(filePath);
                    MyModel.AddShape(filePath, new ColorFillerConst(new Vector3(0.5f, 0.0f, 0.5f)), new ConstNormalVector(), new MPoint(0, 0, 0));
                    CurrentFiller = iNFiller;
                    MyModel.MyShapes[MyModel.MyShapes.Count - 1].CurrentModifier = new ConstNormalVector();
                    Solaris.SetColor(Color.FromArgb(255, 255, 255));
                    MyModel.MyShapes[MyModel.MyShapes.Count - 1].CurrentColor = new ColorFillerConst(new Vector3(0.7f, 0.3f, 0.0f));
                    MyPage.Clera();

                    DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
                    MyPage.DARefresh();
                }
            }

            
        }

        private void StartSunAnimationBt_Click(object sender, EventArgs e)
        {
            MyPage.MySolarTimer.Enabled = MyPage.MySolarTimer.Enabled ? false : true;
            if (MyPage.MySolarTimer.Enabled == false)
            {
                CurrentEngle = 0;
            }
            SetSliderForSun();
        }

        private void MyMovingShapsBt_Click(object sender, EventArgs e)
        {
            MyPage.ThisTimer.Enabled = MyPage.ThisTimer.Enabled ? false : true;
        }
        #endregion

        #region Timer
        private void TimerSolarAnimation_Tick(object sender, EventArgs e)
        {
            float r = 1 * CurrentEngle;

            if (CurrentEngle >= 360 * 3)
            {
                MyPage.MySolarTimer.Enabled = false;
                CurrentEngle = 0;

                SetSliderForSun();
                return;
            }

            float x = r * (float)Math.Cos(CurrentEngle / 180f * Math.PI);
            float y = r * (float)Math.Sin(CurrentEngle / 180f * Math.PI);

            Solaris.X = x;

            Solaris.Y = y;

            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
            CurrentEngle += StepEngle;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            int z = 0;
            foreach(var shp in MyModel.MyShapes)
            {
                if (z == 0)
                 shp.RotateY(0.05f);
                else shp.RotateX(0.05f);

                z++;
            }

            MyPage.Clera();
            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }
        #endregion
        #endregion

        private void ForX_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nm = sender as NumericUpDown;

            PositionCamer.X = (float)nm.Value;
            View = Matrix4x4.CreateLookAt(PositionCamer, new Vector3(0, 0, 1), new Vector3(0, 0, 1));

            MyPage.Clera();
            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }

        private void ForY_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nm = sender as NumericUpDown;

            PositionCamer.Y = (float)nm.Value;
            View = Matrix4x4.CreateLookAt(PositionCamer, new Vector3(0, 0, 1), new Vector3(0, 0, 1));

            MyPage.Clera();
            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }

        private void ForZ_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nm = sender as NumericUpDown;

            PositionCamer.Z = (float)nm.Value;
            View = Matrix4x4.CreateLookAt(PositionCamer, new Vector3(0, 0, 1), new Vector3(0, 0, 1));

            MyPage.Clera();
            DrawAllShapes(MyPage.Canvas, MyModel.MyShapes);
            MyPage.DARefresh();
        }

        #region Sun Functions
        private void CountSunX(float X)
        {
            Solaris.X = X;
            //Solaris.Xf = X / MyModel.Size;
        }

        private void CountSunY(float Y)
        {
            Solaris.Y = Y;
            //Solaris.Yf = Y / MyModel.Size;
        }

        private void CountSunZ(float Z)
        {
            Solaris.Z = Z;
            //Solaris.Zf = Z / MyModel.Size;
        }
        #endregion

        #region Converters
        private float ConvertY(MPoint p, MPoint c)
        {
            return MyPage.CanvasHeight - (p.Y + c.Y + MyPage.CanvasHeight * 0.5f);
        }

        private float ConvertY(float Y, MPoint c)
        {
            return MyPage.CanvasHeight - (Y + c.Y + MyPage.CanvasHeight * 0.5f);
        }

        private float ConvertX(MPoint p, MPoint c)
        {
            return p.X + MyPage.CanvasWidht * 0.5f + c.X;
        }

        private float ConvertX(float X, MPoint c)
        {
            return X + MyPage.CanvasWidht * 0.5f + c.X;
        }
        #endregion

        #region GeneralFunctions
        protected void SetDefaultSettingOfModel()
        {
            MyPage.MyLoadNormalMapCheckB.Checked = false;
            MyPage.MyLoadNormalMapCheckB.Enabled = false;
            //MyModel.MyShape.CurrentModifier = new ConstNormalVector();
            //MyModel.MyShape.CurrentColor = new ColorFillerConst(new Vector3(0.5f, 0.5f, 1));
        }

        protected int Round(float num)
        {
            if (num - (int)num >= 0.5)
            {
                return ((int)num) + 1;
            }

            return (int)num;
        }

        protected void SetSliderForSun()
        {
            if (Solaris.X > MyPage.MySliderH.Maximum)
            {
                MyPage.MySliderH.Value = MyPage.MySliderH.Maximum;
            }
            else if (Solaris.X < MyPage.MySliderH.Minimum)
            {
                MyPage.MySliderH.Value = MyPage.MySliderH.Minimum;
            }
            else
            {
                MyPage.MySliderH.Value = (int)Solaris.X;
            }

            if (Solaris.Y > MyPage.MySliderV.Maximum)
            {
                MyPage.MySliderV.Value = MyPage.MySliderV.Maximum;
            }
            else if (Solaris.Y < MyPage.MySliderV.Minimum)
            {
                MyPage.MySliderV.Value = MyPage.MySliderV.Minimum;
            }
            else
            {
                MyPage.MySliderV.Value = (int)Solaris.Y;
            }
        }
        #endregion
    }

    public class KSDM
    {
        public float ks { get; set; }
        public float kd { get; set; }
        public float m { get; set; }

        public KSDM(float _ks, float _kd, float _m)
        {
            ks = _ks;
            kd = _kd;
            m = _m;
        }
    }
}
