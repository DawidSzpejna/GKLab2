using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.Elements;
using GKLab2.Presenter.Algorithm.Coloring.NormalVectorModifier;
using Model.Elements.Interfaces;

namespace GKLab2.Presenter.Algorithm.Coloring
{
    internal class ConstFiller : BasicFiller
    {
        public ConstFiller(KSDM ksdm) : base(ksdm)
        {
        }
        #region IFiller
        public override void Init(Triangle tr, Sun sun, MPoint center, INormalModifier mod, IColorFiller cl)
        {
            base.Init(tr, sun, center, mod, cl);
        }

        public override Color ProperColour(float x, float y)
        {
            Vector3 result = CFiller.GiveMeColor(x, y);

            int r = (int)(result.X * MaxColor);
            int g = (int)(result.Y * MaxColor);
            int b = (int)(result.Z * MaxColor);

            if (r > 255) r = 255;
            if (g > 255) g = 255;
            if (b > 255) b = 255;

            return Color.FromArgb(r, g, b);
        }
        #endregion
    }
}
