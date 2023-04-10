using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements.Interfaces;
using System.Numerics;

namespace Model.Elements
{
    public class Shape
    {
        #region Properties
        public HashSet<Edge> Edges { get; }
        public List<Triangle> Triangles { get; }
        public MPoint Center { get; set; }
        public float AngleX { get; set; }
        public float AngleY { get; set; }
        public float AngleZ { get; set; }

        public IColorFiller CurrentColor { get; set; }
        public INormalModifier CurrentModifier { get; set; }


        public Matrix4x4 Model { get; set; }
        #endregion

        #region Constructors
        public Shape()
        {
            Edges = new HashSet<Edge>();
            Triangles = new List<Triangle>();
            Center = new MPoint(0, 0, 0);
            AngleX = 0;
            AngleY = 0;
            AngleZ = 0;

            Model = Matrix4x4.Identity;
        }

        public Shape(float x, float y, float z)
        {
            Edges = new HashSet<Edge>();
            Triangles = new List<Triangle>();
            Center = new MPoint(z, y, z);
            AngleX = 0;
            AngleY = 0;
            AngleZ = 0;

            Model = Matrix4x4.Identity;
        }
        #endregion

        #region CommonFeatures
        public void Add(Triangle t)
        {
            Edges.Add(t.E1);
            Edges.Add(t.E2);
            Edges.Add(t.E3);

            Triangles.Add(t);
        }
        #endregion


        #region CountView

        public MPoint PipeOfView(Matrix4x4 View, Matrix4x4 Projection, MPoint p, float width, float height)
        {
            Vector4 vec = new Vector4(p.X, p.Y, p.Z, 1);

            Vector4 af1 = Vector4.Transform(vec, Model);
            Vector4 af2 = Vector4.Transform(af1, View);
            Vector4 af3 = Vector4.Transform(af2, Projection);

            if (af3.W != 0) af3 /= af3.W;

            Vector3 tmp_norm = Vector3.TransformNormal(p.Norm, Model);
            MPoint tmp_p = new MPoint(af3.X * (width / 2f), af3.Y * (height / 2f), af3.Z * (width / 2f));
            tmp_p.Norm = tmp_norm;

            return tmp_p;
        }

        public void RotateX(float addAngle)
        {
            AngleX += addAngle;
            Model = Matrix4x4.CreateRotationX(AngleX);
        }

        public void RotateY(float addAngle)
        {
            AngleY += addAngle;
            Model = Matrix4x4.CreateRotationY(AngleY);
        }

        public void RotateZ(float addAngle)
        {
            AngleZ += addAngle;
            Model = Matrix4x4.CreateRotationZ(AngleZ);
        }
        #endregion
    }
}
