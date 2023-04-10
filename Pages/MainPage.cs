using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pages
{
    public partial class MainPage : Form
    {
        #region Properties
        public PictureBox MyDrawingArea => DrawingArea;
        public int CanvasWidht => Canvas.Width;
        public int CanvasHeight => Canvas.Height;
        public Bitmap Canvas { get; private set; }

        #region TrackBar
        public TrackBar MySliderH => SliderH;
        public TrackBar MySliderV => SliderV;
        public TrackBar MySliderZ => SliderZ;
        public TrackBar MySliderKS => SliderKS;
        public TrackBar MySliderKD => SliderKD;
        public TrackBar MySliderM => SliderM;
        public TrackBar MySliderFov => SliderFov;
        #endregion

        #region CheckBox
        public CheckBox MyCheckBoxGrid => CheckBoxGrid;
        public CheckBox MyLoadNormalMapCheckB => TurnOfMapCheckBox;
        #endregion

        #region RadioButtons
        public RadioButton MyVectorMode => RadioButtonVctr;
        public RadioButton MyVertexMode => RadioButtonVrtx;
        public RadioButton MyHRBt => HRBt;
        public RadioButton MyNRBt => NRBt;
        #endregion

        #region NumericUpDown
        public NumericUpDown MForX => ForX;
        public NumericUpDown MForY => ForY;
        public NumericUpDown MForZ => ForZ;
        #endregion

        #region Buttons
        public Button MySellectObjectColorBt => SellectObjectColorBt;
        public Button MySellectLightColorBt => SellectLightColorBt;
        public Button MyLoadNormalMapBt => LoadNormalMapBt;
        public Button MyLoadTextureBt => LoadTextureBt;
        public Button MyLoadShapeBt => LoadShapeBt;
        public Button MyStartSunAnimationBt => StartSunAnimationBt;
        public Button MyMovingShapsBt => MovingShapsBt;
        #endregion

        #region Labels
        public Label MyKsLabel => KsLabel;
        public Label MyKdLabel => KdLabel;
        public Label MyMLabel => MLabel;
        #endregion

        #region Timer
        public System.Windows.Forms.Timer MySolarTimer => TimerSolarAnimation;
        public System.Windows.Forms.Timer ThisTimer => timer1;
        #endregion

        #endregion

        #region Constructors
        public MainPage()
        {
            InitializeComponent();

            #region Setting Slider and Labels
            {
                int MaxH = (int)(SliderH.Maximum);
                int MaxV = (int)(SliderV.Maximum);
                int min = MaxH > MaxV ? MaxV : MaxH;

                //SliderH.Minimum = -MaxH;
                //SliderH.Maximum = MaxH;

                //SliderV.Minimum = -MaxV;
                //SliderV.Maximum = MaxV;

                //SliderZ.Minimum = min + 1;
                //SliderZ.Maximum = 2 * min;

                SliderH.Minimum = -2000;
                SliderH.Maximum = 2000;

                SliderV.Minimum = -2000;
                SliderV.Maximum = 2000;

                SliderZ.Minimum = 251;
                SliderZ.Maximum = 2000;

                SliderKS.Value = SliderKD.Value = 50;
                KsLabel.Text = KdLabel.Text = "0.5";

                SliderM.Value = 1;
                MLabel.Text = "1";

                SliderH.Value = SliderV.Value = 0;
                SliderZ.Value = (int)(min * 1.2f);

                SliderFov.Value = 30;
            }
            #endregion

            Canvas = new Bitmap(DrawingArea.Width, DrawingArea.Height);
            DrawingArea.Image = Canvas;
            using (Graphics g = Graphics.FromImage(Canvas))
            {
                g.Clear(Color.LightGray);
            }
            DrawingArea.Refresh();
        }
        #endregion

        public void DARefresh()
        {
            DrawingArea.Refresh();
        }

        public void Clera()
        {
            using (Graphics g = Graphics.FromImage(Canvas))
            {
                g.Clear(Color.LightGray);
            }
        }
    }
}
