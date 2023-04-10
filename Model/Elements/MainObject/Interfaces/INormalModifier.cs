using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Model.Elements.Interfaces
{
    public interface INormalModifier
    {
        public Vector3 ModifyN(Vector3 N, float x, float y);
    }
}
