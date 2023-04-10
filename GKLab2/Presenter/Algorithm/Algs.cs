using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GKLab2.Presenter.Algorithm.Structures;
using Model.Elements;
using GKLab2.Presenter.Algorithm.Coloring;
using System.Numerics;

namespace GKLab2.Presenter.Algorithm
{
    public static class Algs
    {
        public static void FillPolygonAlg(Presenter presenter, Triangle tr, MPoint center)
        {
            ET myET = new ET();
            myET.FillTable(tr);

            int minY = myET.MinIndex;
            List<AETnode> myAET = new List<AETnode>();

            while (!(myET.Count == 0 && myAET.Count == 0))
            {
                List<AETnode> next_tmp = myET.ReturnEAT(minY);
                if (next_tmp != null)
                {
                    myAET = new List<AETnode>(myAET.Concat(next_tmp));
                }

                myAET = new List<AETnode>(myAET.OrderBy(n => n.Xcurrent));
                myAET.RemoveAll(n => minY == Math.Round(n.Ymax));
                presenter.DrawLines(tr, center, myAET, minY);

                //ClearMe(myAET, minY);
                minY++;
                IncreaseAll(myAET);
            }

            //// Fill vertical edges
            //if (tr.E1.A.Y == tr.E1.B.Y)
            //{
            //    presenter.DrawLine(tr.E1.A.X, tr.E1.B.X, (int)tr.E1.A.Y);

            //}
            //else if (tr.E2.A.Y == tr.E2.B.Y)
            //{
            //    presenter.DrawLine(tr.E2.A.X, tr.E2.B.X, (int)tr.E2.A.Y);
            //}
            //else if (tr.E3.A.Y == tr.E3.B.Y)
            //{
            //    presenter.DrawLine(tr.E3.A.X, tr.E3.B.X, (int)tr.E3.A.Y);
            //}

        }

        private static void ClearMe(List<AETnode> list, int minY)
        {
            //LinkedListNode<AETnode> node = list.First;
            //LinkedListNode<AETnode> next_node = null;

            //while (node != null)
            //{
            //    next_node = node.Next;
            //    if ((int)node.Value.Ymax <= minY)
            //    {
            //        list.Remove(node);
            //    }
            //    node = next_node;
            //}
        }

        private static void IncreaseAll(List<AETnode> list)
        {
            foreach(AETnode node in list)
            {
                node.Icrease();
            }
        }

        public static int Round(float num)
        {
            if (num - (int)num >= 0.5)
            {
                return ((int)num) + 1;
            }

            return (int)num;
        }
    }
}
