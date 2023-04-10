using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKLab2.Presenter.Algorithm.Structures
{
    public class AETnode
    {
        private float _Ymax;
        private float _Xcurrent;
        private float _Factor;

        #region Getters
        public float Ymax => _Ymax;
        public float Xcurrent => _Xcurrent;
        public float Factor => _Factor;
        #endregion

        #region Construtctors
        public AETnode(Model.Elements.Edge e)
        {
            _Ymax = e.A.Y;
            _Xcurrent = e.B.X;
            if (Ymax < e.B.Y)
            {
                _Ymax = e.B.Y;
                _Xcurrent = e.A.X;
            }

            if (e.A.X == e.B.Y)
            {
                _Factor = 0;
                return;
            }

            _Factor = (e.A.Y - e.B.Y) / (e.A.X - e.B.X);
        }

        public AETnode(float x)
        {
            _Xcurrent = x;
        }
        #endregion

        #region Common Features
        public void Icrease()
        {
            if (_Factor.Equals(0)) return;
            float _m = 1 / _Factor;
            _Xcurrent += _m;
        }
        #endregion
    }
}
