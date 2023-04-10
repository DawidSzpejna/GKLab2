using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace Model.Elements
{
    public class Sun : MPoint
    {
        #region Properties
        public Vector3 IL; // The color of light
        #endregion

        #region Constructors
        public Sun(float x, float y, float z,
                      float xf, float yf, float zf, Color C) : base(x, y, z)
        {
            //Xf = xf;
            //Yf = yf;
            //Zf = zf;

            SetColor(C);
        }

        public Sun(float x, float y, float z,
                      float xf, float yf, float zf, Vector3 C) : base(x, y, z)
        {
            //Xf = xf;
            //Yf = yf;
            //Zf = zf;

            IL = C;
        }

        public Sun(float x, float y, float z) : base(x, y, z)
        {

        }
        public Sun(float x, float y) : base(x, y, 0) 
        { }
        public Sun(float x, float y, Vector3 norm) : base(x, y)
        {
            Norm = norm;
        }
        #endregion

        #region Common Features
        public void SetColor(Color c)
        {
            IL.X = c.R / 255f;
            IL.Y = c.G / 255f;
            IL.Z = c.B / 255f;
        }
        #endregion
    }
}
