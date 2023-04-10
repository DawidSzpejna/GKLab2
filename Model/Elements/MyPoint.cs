using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Model.Elements
{
    public class MPoint
    {
        #region Properties
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Xf { get; set; }
        public float Yf { get; set; }
        public float Zf { get; set; }

        public Vector3 Norm { get; set; }
        #endregion

        #region Constructors
        //public MPoint(float x, float y, float z,
        //              float xf, float yf, float zf) : this(x,y,z)
        //{
        //    //Xf = xf;
        //    //Yf = yf;
        //    //Zf = zf;
        //}

        public MPoint(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public MPoint(float x, float y) : this(x, y, 0) { }

        public MPoint(float x, float y, Vector3 norm) : this(x, y)
        {
            Norm = norm;
        }
        #endregion
    }
}
