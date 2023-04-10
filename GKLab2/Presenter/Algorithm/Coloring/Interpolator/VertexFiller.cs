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
    public class IVFiller : BasicFiller
    {
        #region Fields
        private Vector3 ColorP1, ColorP2, ColorP3;
        #endregion

        #region Constructors
        public IVFiller(KSDM ksdm) : base(ksdm)
        {
            ColorP1 = ColorP2 = ColorP3 = new Vector3();
        }
        #endregion

        #region IFiller
        public override void Init(Triangle tr, Sun sun, MPoint center, INormalModifier mod, IColorFiller cl)
        {
            base.Init(tr, sun, center, mod, cl);


            // P1
            {
                Vector3 IO = CFiller.GiveMeColor(Tr.P1.X, Tr.P1.Y);
                Vector3 L = GiveMeL(Tr.P1, center), N = Tr.P1.Norm;

                // ------------
                N = Mod.ModifyN(N, Tr.P1.X, Tr.P1.Y);
                // ------------

                ColorP1 = ThisColor(Ksdm.kd, Ksdm.ks, Ksdm.m, MySun.IL, IO, L, N);
            }

            // P2
            {
                Vector3 IO = CFiller.GiveMeColor(Tr.P2.X, Tr.P2.Y);
                Vector3 L = GiveMeL(Tr.P2, center), N = Tr.P2.Norm;

                // ------------
                N = Mod.ModifyN(N, Tr.P2.X, Tr.P2.Y);
                // ------------

                ColorP2 = ThisColor(Ksdm.kd, Ksdm.ks, Ksdm.m, MySun.IL, IO, L, N);
            }

            // P3
            {
                Vector3 IO = CFiller.GiveMeColor(Tr.P3.X, Tr.P3.Y);
                Vector3 L = GiveMeL(Tr.P3, center), N = Tr.P3.Norm;

                // ------------
                N = Mod.ModifyN(N, Tr.P3.X, Tr.P3.Y);
                // ------------

                ColorP3 = ThisColor(Ksdm.kd, Ksdm.ks, Ksdm.m, MySun.IL, IO, L, N);
            }
        }

        public override Color ProperColour(float x, float y)
        {
            MPoint P = new MPoint(x, y);
            (float u, float v, float w) = GiveMeUVW_withoutZ(Tr, P);

            Vector3 vec = BarycentricInterpolation(ColorP1, ColorP2, ColorP3, u, v, w);

            //if (vec.X < 0) vec.X *= -1;
            //if (vec.Y < 0) vec.Y *= -1;
            //if (vec.Z < 0) vec.Z *= -1;

            return Color.FromArgb((int)((vec.X > 1 ? 1 : vec.X) * MaxColor), (int)((vec.Y > 1 ? 1 : vec.Y) * MaxColor), (int)((vec.Z > 1 ? 1 : vec.Z) * MaxColor));
        }
        #endregion
    }
}
