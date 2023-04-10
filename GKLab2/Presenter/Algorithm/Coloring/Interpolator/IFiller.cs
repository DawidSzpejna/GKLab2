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
    public interface IFiller
    {
        Color ProperColour(float x, float y);
        void Init(Triangle tr, Sun sun, MPoint center, INormalModifier mod, IColorFiller cl);
    }
}
