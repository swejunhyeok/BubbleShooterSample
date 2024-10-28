using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class CellEffect : CellComponent
        {
            public void RunBombEffect()
            {
                LayerType hitLayer = LayerType.Bottom | LayerType.Middle | LayerType.Top;
                for(int i = 0; i < Parent.ArroundCell.Count; ++i)
                {
                    if (Parent.ArroundCell[i] == null)
                    {
                        continue;
                    }

                    Parent.ArroundCell[i].Block.Hit(hitLayer, BlockType.BombCircle, HitConditionType.SpecialBlock);
                }
            }

            public void RunLargeBombEffect()
            {
                LayerType hitLayer = LayerType.Bottom | LayerType.Middle | LayerType.Top;
                List<Cell> visitCell = new List<Cell>();
                for (int i = 0; i < Parent.ArroundCell.Count; ++i)
                {
                    if (visitCell.IndexOf(Parent.ArroundCell[i]) != -1)
                    {
                        continue;
                    }
                    visitCell.Add(Parent.ArroundCell[i]);
                    if (Parent.ArroundCell[i] == null)
                    {
                        continue;
                    }
                    Parent.ArroundCell[i].Block.Hit(hitLayer, BlockType.BigBombCircle, HitConditionType.SpecialBlock);
                    for (int j = 0; j < Parent.ArroundCell[i].ArroundCell.Count; ++j)
                    {
                        if (visitCell.IndexOf(Parent.ArroundCell[i].ArroundCell[j]) != -1)
                        {
                            continue;
                        }
                        visitCell.Add(Parent.ArroundCell[i].ArroundCell[j]);
                        if (Parent.ArroundCell[i].ArroundCell[j] == null)
                        {
                            continue;
                        }
                        Parent.ArroundCell[i].ArroundCell[j].Block.Hit(hitLayer, BlockType.BigBombCircle, HitConditionType.SpecialBlock);
                    }
                }
            }
        }
    }
}
