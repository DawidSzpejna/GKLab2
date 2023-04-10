using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Elements
{
    public class Triangle
    {
        public Edge E1, E2, E3;
        public MPoint P1, P2, P3;

        #region Constructors
        public Triangle(Edge e1, Edge e2, Edge e3)
        {
            E1 = e1;
            E2 = e2;
            E3 = e3;
        }

        public Triangle(Edge e1, Edge e2, Edge e3,
                        MPoint p1, MPoint p2, MPoint p3) : this(e1, e2, e3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }
        #endregion
    }
}
