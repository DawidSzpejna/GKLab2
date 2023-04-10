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
    public abstract class BasicFiller : IFiller
    {
        #region Fields
        public readonly Vector3 V;
        public const int MaxColor = 255;
        #endregion

        #region Properites
        protected IColorFiller CFiller { get; private set; }
        protected KSDM Ksdm { get; private set; }
        protected Triangle Tr { get; private set; }
        protected Sun MySun { get; private set; }
        protected MPoint Center { get; private set; }
        protected INormalModifier Mod { get; private set; }
        #endregion

        #region Constructuros
        public BasicFiller(KSDM ksdm)
        {
            V = new Vector3(0, 0, 1);
            Ksdm = ksdm;
        }
        #endregion

        #region IFiller
        public virtual Color ProperColour(float x, float y)
        {
            return new Color();
        }

        public virtual void Init(Triangle tr, Sun sun, MPoint center, INormalModifier mod, IColorFiller cl)
        {
            Tr = tr;
            MySun = sun;
            Mod = mod;
            CFiller = cl;
            Center = center;
        }
        #endregion

        #region Calculationg Colors
        public Vector3 ThisColor(float kd, float ks, float m,
                                        Vector3 IL, Vector3 IO,
                                        Vector3 L, Vector3 N)
        {
            Vector3 R = 2 * Vector3.Dot(N, L) * N - L;

            float cos_1 = Vector3.Dot(N, L);
            float cos_2 = Vector3.Dot(V, R);

            if (cos_1 < 0) cos_1 = 0;
            if (cos_2 < 0) cos_2 = 0;

            if (cos_1 > 0 || cos_2 > 0)
            {
                int dupek = 0;
                dupek++;
            }

            Vector3 result = IO * IL * (kd * cos_1 + ks * (float)Math.Pow(cos_2, m));

            return result;
        }

        public static Vector3 BarycentricInterpolation(MPoint A, MPoint B, MPoint C,
                                                float u, float v, float w)
        {
            Vector3 P1 = new Vector3(A.X, A.Y, A.Z);
            Vector3 P2 = new Vector3(B.X, B.Y, B.Z);
            Vector3 P3 = new Vector3(C.X, C.Y, C.Z);

            return BarycentricInterpolation(P1, P2, P3, u, v, w);
        }

        public static Vector3 BarycentricInterpolation(Vector3 A, Vector3 B, Vector3 C,
                                                        float u, float v, float w)
        {
            Vector3 vec = u * A + v * B + w * C;
            return vec;
        }

        public Vector3 GiveMeL(MPoint p, MPoint center)
        {
            Vector3 pp = new Vector3(MySun.X - p.X - center.X , MySun.Y - p.Y - center.Y, MySun.Z - p.Z - center.Z);
            return Vector3.Normalize(pp);
        }
        #endregion

        #region U,V,W counting
        public (float u, float v, float w) GiveMeUVW(Triangle tr, MPoint P)
        {
            float Dot(MPoint P1, MPoint P2, MPoint P3)
                => (P2.X - P1.X) * (P3.X - P1.X) +
                   (P2.Y - P1.Y) * (P3.Y - P1.Y) +
                   (P2.Z - P1.Z) * (P3.Z - P1.Z);

            float Area = Dot(tr.P1, tr.P2, tr.P3) * 0.5f;

            return (0.5f * Dot(tr.P2, tr.P3, P) / Area,
                    0.5f * Dot(tr.P1, tr.P3, P) / Area,
                    0.5f * Dot(tr.P1, P, tr.P2) / Area);
        }
        public static (float u, float v, float w) GiveMeUVW_withoutZ(Triangle tr, MPoint P)
        {
            float Area = CountTriangle(tr.P1, tr.P2, tr.P3);
            if (Area == 0)
            {
                return (0, 0, 0);
            }

            float a1 = CountTriangle(tr.P2, tr.P3, P); // u 
            float a2 = CountTriangle(tr.P1, tr.P3, P); // v 
            float a3 = CountTriangle(tr.P1, P, tr.P2); // w

            Area = a1 + a2 + a3;

            float u = a1 / Area;
            float v = a2 / Area;
            float w = a3 / Area;
            //float w = 1 - u - v;
            //if (w < 0) w = 0;

            float z = u + v + w;

            return ( u,
                     v,
                     w);
        }
        public static float CountTriangle(MPoint p1, MPoint p2, MPoint p3)
        {
            float Dot2(MPoint P1, MPoint P2, MPoint P3)
                => (P2.X - P1.X) * (P3.Y - P1.Y) -
                   (P2.Y - P1.Y) * (P3.X - P1.X);

            if (p1.Equals(p2) || p2.Equals(p3) || p3.Equals(p1)) return 0;

            if (p1.X.Equals(p2.X) && p2.X.Equals(p3.X)) return 0;
            if (p1.Y.Equals(p2.Y) && p2.Y.Equals(p3.Y)) return 0;

            float Area = Math.Abs(Dot2(p1, p2, p3));
            if (Area == 0)
            {
                float a = (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
                float b = (float)Math.Sqrt(Math.Pow(p1.X - p3.X, 2) + Math.Pow(p1.Y - p3.Y, 2));
                Area = a * b;
            }

            return Area;
        }
        #endregion
    }
}
