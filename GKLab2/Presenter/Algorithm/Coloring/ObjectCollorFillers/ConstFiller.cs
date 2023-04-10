using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.Elements.Interfaces;

namespace GKLab2.Presenter.Algorithm.Coloring
{
    public class ColorFillerConst : IColorFiller
    {
        #region Fields
        protected Vector3 Color;
        #endregion

        #region Constructors
        public ColorFillerConst(Vector3 color)
        {
            this.Color = color;
        }
        public ColorFillerConst(Color color)
        {
            this.Color.X = color.R / 255f;
            this.Color.Y = color.G / 255f;
            this.Color.Z = color.B / 255f;
        }
        #endregion

        #region ICollorFiller
        public Vector3 GiveMeColor(float x, float y)
        {
            return Color;
        }
        #endregion
    }
}
