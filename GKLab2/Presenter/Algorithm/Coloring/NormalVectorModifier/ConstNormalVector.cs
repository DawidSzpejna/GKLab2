using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.Elements.Interfaces;

namespace GKLab2.Presenter.Algorithm.Coloring.NormalVectorModifier
{
    public class ConstNormalVector : INormalModifier
    {
        #region INormalModifier
        public Vector3 ModifyN(Vector3 N, float x, float y)
        {
            return N;
        }
        #endregion
    }
}
