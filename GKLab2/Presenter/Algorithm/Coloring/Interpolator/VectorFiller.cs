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
    public class INFiller : BasicFiller
    {
        #region Fields
        private Vector3 Sol1, Sol2, Sol3;
        #endregion

        #region Constructors
        public INFiller(KSDM ksdm) : base(ksdm) {}
        #endregion

        #region IFiller
        public override void Init(Triangle tr, Sun sun, MPoint center, INormalModifier mod, IColorFiller cl)
        {
            base.Init(tr, sun, center, mod, cl);

            Sol1 = GiveMeL(tr.P1, center);
            Sol2 = GiveMeL(tr.P2, center);
            Sol3 = GiveMeL(tr.P3, center);
        }

        public override Color ProperColour(float x, float y)
        {
            MPoint P = new MPoint(x, y);
            (float u, float v, float w) = GiveMeUVW_withoutZ(Tr, P);



            Vector3 sol = BarycentricInterpolation(Sol1, Sol2, Sol3, u, v, w);
            sol = Vector3.Normalize(sol);
            Vector3 norm = BarycentricInterpolation(Tr.P1.Norm, Tr.P2.Norm, Tr.P3.Norm, u, v, w);
            norm = Vector3.Normalize(norm);

            Vector3 IO = CFiller.GiveMeColor(x, y);
            // ------------
            norm = Mod.ModifyN(norm, x, y);
            // ------------

            Vector3 result = ThisColor(Ksdm.kd, Ksdm.ks, Ksdm.m, MySun.IL, IO, sol, norm);

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
