using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKLab2.Presenter.Algorithm.Structures
{
    public class ET
    {
        private Dictionary<int, List<AETnode>> _RangeYY;
        private int _MinIndex;
        private int _Count;

        #region Properties
        public Dictionary<int, List<AETnode>> RangeYY => _RangeYY;

        public int MinIndex => _MinIndex;
        public int Count => _Count;
        #endregion

        #region Constructors
        public ET()
        {
            _RangeYY = new Dictionary<int, List<AETnode>>();
            _MinIndex = int.MaxValue;
            _Count = 0;
        }
        #endregion

        #region Main Features
        public void FillTable(List<Model.Elements.Edge> edges)
        {
            
            foreach (Model.Elements.Edge e in edges)
            {
                AETnode tmp = new AETnode(e);
                int ymin = (int)Math.Round(e.A.Y > e.B.Y ? e.B.Y : e.A.Y);

                if (ymin < _MinIndex)
                {
                    _MinIndex = ymin;
                }

                if (RangeYY.ContainsKey(ymin) == false)
                {
                    List<AETnode> tmp_list = new List<AETnode>();
                    tmp_list.Add(tmp);
                    RangeYY.Add(ymin, tmp_list);
                    _Count++;
                }
                else
                {
                    RangeYY[ymin].Add(tmp);
                }
            }
        }

        public void FillTable(Model.Elements.Triangle tr)
        {
            List<Model.Elements.Edge> edgeList = new List<Model.Elements.Edge>();
            edgeList.Add(tr.E1);
            edgeList.Add(tr.E2);
            edgeList.Add(tr.E3);

            FillTable(edgeList);
        }

        public List<AETnode> ReturnEAT(int y)
        {
            if (!_RangeYY.ContainsKey(y)) return null;

            List<AETnode> tmp = _RangeYY[y];
            _RangeYY.Remove(y);
            _Count--;

            return tmp;
        }
        #endregion
    }
}
