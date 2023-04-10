using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Model.Elements
{
    public class Edge
    {
        public MPoint A { get; }
        public MPoint B { get; }

        #region Constructors
        public Edge(MPoint a, MPoint b)
        {
            A = a;
            B = b;
        }

        public Edge(float x1, float y1, Vector3 norm1, float x2, float y2, Vector3 norm2)
        {
            A = new MPoint(x1, y1, norm1);
            B = new MPoint(x2, y2, norm2);
        }
        #endregion
    }
}
