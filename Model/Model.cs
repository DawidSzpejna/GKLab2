using Model.Elements;
using Model.Elements.Interfaces;

namespace Model
{
    public class GenerelModel
    {
        #region Fields
        private string _PthDefaultShape2 = @"DefaultShapes\rightSphere.obj";
        private string _PthDefaultShape3 = @"DefaultShapes\flipedNormals.obj";
        private string _PthDefaultShape4 = @"DefaultShapes\TriangleTest.obj";
        private string _PthDefaultShape5 = @"DefaultShapes\paczek.obj";
        private string _PthDefaultShape6 = @"DefaultShapes\FullDonut.obj";
        #endregion

        #region Properties
        public List<Shape> MyShapes { get; private set; }
        public float Size { get; private set; }
        public const float Margin = 150;
        public string PthDefaultShape => _PthDefaultShape2;
        #endregion

        #region Constructors
        public GenerelModel()
        {
            MPoint p1 = new MPoint(100, 400);
            MPoint p2 = new MPoint(300, 200);
            MPoint p3 = new MPoint(400, 200);
            Triangle tr = new Triangle(new Edge(p1, p2), new Edge(p1, p3), new Edge(p2, p3), p1, p2, p3);

            Shape tmp = new Shape();
            tmp.Add(tr);

            MyShapes = new List<Shape>();
            MyShapes.Add(tmp);
        }

        public GenerelModel(int width, int height)
        {
            //width > height ? height * 0.5f : width * 0.5f;
            Size = width > height ? height * 0.5f : width * 0.5f;
            Size -= Margin;

            Shape tmp = ObjParser.PareOjeFile(_PthDefaultShape6, Size);
            MyShapes = new List<Shape>();
            MyShapes.Add(tmp);
        }

        public GenerelModel(string path, int width, int height)
        {
            Shape tmp = ObjParser.PareOjeFile(path, Size);
            MyShapes = new List<Shape>();
            MyShapes.Add(tmp);
        }
        #endregion

        #region Common Features
        public void AddShape(string path, IColorFiller color, INormalModifier modifier, MPoint center)
        {
            Shape tmp = ObjParser.PareOjeFile(path, Size);
            tmp.CurrentModifier = modifier;
            tmp.CurrentColor = color;
            tmp.Center = center;
            tmp.RotateX(0.8f); // <----- dodane na czas labów

            MyShapes.Add(tmp);
        }

        public void Reload(string path)
        {
            Shape tmp = ObjParser.PareOjeFile(path, Size);
            MyShapes = new List<Shape>();
            MyShapes.Add(tmp);
        }

        public void Clear()
        {
            MyShapes.Clear();
        }
        #endregion
    }
}