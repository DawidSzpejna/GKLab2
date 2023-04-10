using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.Elements.Interfaces;

namespace GKLab2.Presenter.Algorithm.Coloring.NormalVectorModifier
{
    public class ByHightMapModifier : INormalModifier
    {
        #region Properties
        private float[,] HightMap { get; set; }
        Bitmap HighMapp { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }
        private float MaxColor => 255;
        #endregion


        public ByHightMapModifier(Bitmap highMap)
        {
            HighMapp = highMap;
            Width = highMap.Width;
            Height = highMap.Height;

        }

        public Vector3 ModifyN(Vector3 N, float x, float y)
        {
            Vector3 helper = new Vector3(0, 0, 1);
            Vector3 B = Vector3.Cross(N, helper);
            if (N.X == 0 && N.Y == 0 && N.Z == 1) B = new Vector3(0, 1, 0);

            Vector3 T = Vector3.Cross(B, N);

            x = convertX(x);
            y = convertY(y);

            Color CurrentC = HighMapp.GetPixel((int)Math.Round(x), (int)Math.Round(y));
            Color NextY = HighMapp.GetPixel((int)Math.Round(x), (int)Math.Round(y + 1));
            Color NextX = HighMapp.GetPixel((int)Math.Round(x + 1), (int)Math.Round(y));

            float delta_x = NextX.R - CurrentC.R;
            float delta_y = NextY.R - CurrentC.R;
            delta_x /= 18;
            delta_y /= 18;

            Vector3 D = T * delta_x + B * delta_y;

            D = D * 1f;
            return Vector3.Normalize(N + D);
        }

        public float convertX(float x)
        {
            return (x + Width / 2f);
        }

        public float convertY(float y)
        {
            return (Height - (y + Height / 2f));
        }
    }
}
