using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Model.Elements.Interfaces
{
    public interface IColorFiller
    {
        public Vector3 GiveMeColor(float x, float y);
    }
}
