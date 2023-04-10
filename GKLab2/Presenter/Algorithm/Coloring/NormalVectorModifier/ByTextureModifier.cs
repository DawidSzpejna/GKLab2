using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.Elements.Interfaces;

namespace GKLab2.Presenter.Algorithm.Coloring.NormalVectorModifier
{
    public class ByTextureModifier : INormalModifier
    {
        #region Properties
        private Vector3[,] NormalMap { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }
        private float MaxColor => 255;
        #endregion

        #region Constructors
        public ByTextureModifier(Bitmap normalMap)
        {
            Width = normalMap.Width;
            Height = normalMap.Height;

            NormalMap = new Vector3[Height, Width];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    SetVector(i, j, normalMap.GetPixel(j, i));
                }
            }
        }
        #endregion

        #region Common Features
        private void SetVector(int i, int j, Color c)
        {
            NormalMap[i, j].X = 2f * (c.R / MaxColor) - 1;
            NormalMap[i, j].Y = 2f * (c.G / MaxColor) - 1;
            NormalMap[i, j].Z = 2f * (c.B / MaxColor) - 1;
        }

        private void GetVector(float x, float y)
        {

        }

        public float convertX(float x)
        {
            return x + Width / 2f;
        }

        public float convertY(float y)
        {
            return Height - (y + Height / 2f);
        }
        #endregion

        #region INormalModifier
        public Vector3 ModifyN(Vector3 N, float x, float y)
        {
            // .... N = M * NT ....

            // Getting NT
            float _x = convertX(x);
            float _y = (int)convertY(y);

            _x = _x - Width * (int)Math.Floor(_x / Width);
            _y = _y - Height * (int)Math.Floor(_y / Height);

            Vector3 NT = NormalMap[(int)_y, (int)_x];

            // Making elements of M
            Vector3 helper = new Vector3(0, 0, 1);
            Vector3 B = Vector3.Cross(N, helper);
            if (N.X == 0 && N.Y == 0 && N.Z == 1) B = new Vector3(0, 1, 0);

            Vector3 T = Vector3.Cross(B, N);

            // Counting M * NT
            Vector3 result = new Vector3(T.X * NT.X + B.X * NT.X + N.X * NT.X,
                                         T.Y * NT.Y + B.Y * NT.Y + N.Y * NT.Y,
                                         T.Z * NT.Z + B.Z * NT.Z + N.Z * NT.Z);

            return result;
        }
        #endregion
    }
}
