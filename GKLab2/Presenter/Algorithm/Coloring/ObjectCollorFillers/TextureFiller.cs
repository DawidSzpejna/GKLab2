using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.Elements.Interfaces;

namespace GKLab2.Presenter.Algorithm.Coloring.CollorFillers
{
    public class ColorFillerTexture : IColorFiller
    {
        #region Properties
        private Bitmap Texture { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }
        private float MaxColor => 255;
        #endregion

        #region Constructors
        public ColorFillerTexture(Bitmap texture)
        {
            Texture = texture;
            Width = texture.Width;
            Height = texture.Height;
        }
        #endregion

        #region ICollorFiller
        public Vector3 GiveMeColor(float x, float y)
        {
            float _x = convertX(x);
            float _y = (int)convertY(y);

            _x = _x - Width * (int)Math.Floor(_x / Width);
            _y = _y - Height * (int)Math.Floor(_y / Height);

            Color c = Texture.GetPixel((int)_x, (int)_y);

            return new Vector3(c.R / MaxColor, c.G / MaxColor, c.B / MaxColor);
        }
        #endregion

        public float convertX(float x)
        {
            return x + Width / 2f;
        }

        public float convertY(float y)
        {
            return Height - (y + Height / 2f);
        }
    }
}
