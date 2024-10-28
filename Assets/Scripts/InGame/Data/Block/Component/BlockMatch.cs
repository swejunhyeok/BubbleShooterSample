using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class BlockMatch : BlockComponent
        {

            public void RunMatch()
            {
                List<Cell> visitCell = new List<Cell>() { Parent.PivotCell};
                List<Cell> sameColorCell = new List<Cell>() { Parent.PivotCell};

                CheckArroundCell(Parent.PivotCell, ref visitCell, ref sameColorCell);

                if(sameColorCell.Count >= 3)
                {
                    List<Vector2Int> hitPosition = new List<Vector2Int>();
                    for(int i = 0; i < sameColorCell.Count; ++i)
                    {
                        hitPosition.Add(sameColorCell[i].Pos);
                    }
                    for(int i = 0; i < sameColorCell.Count; ++i)
                    {
                        sameColorCell[i].Block.Hit(LayerType.Middle, Parent.Attribute.Type, HitConditionType.ColorMatch, hitPosition);
                    }
                }
            }

            private void CheckArroundCell(Cell pivotCell, ref List<Cell> visitCell, ref List<Cell> sameColorCell)
            {
                for(int i = 0; i < pivotCell.ArroundCell.Count; ++i)
                {
                    if (visitCell.IndexOf(pivotCell.ArroundCell[i]) != -1)
                    {
                        continue;
                    }
                    visitCell.Add(pivotCell.ArroundCell[i]);
                    if (pivotCell.ArroundCell[i] == null)
                    {
                        continue;
                    }

                    if (IsSameColor(pivotCell.ArroundCell[i]))
                    {
                        sameColorCell.Add(pivotCell.ArroundCell[i]);
                        CheckArroundCell(pivotCell.ArroundCell[i], ref visitCell, ref sameColorCell);
                    }
                }
            }

            public bool IsSameColor(Cell targetCell)
            {
                if(targetCell == null)
                {
                    return false;
                }
                if(!targetCell.Block.HasMiddleBlock)
                {
                    return false;
                }
                if(!targetCell.Block.MiddleBlock.HasAttribute)
                {
                    return false;
                }
                if(targetCell.Block.MiddleBlock.State.State != BlockStateType.Idle)
                {
                    return false;
                }
                return Parent.Attribute.Color == targetCell.Block.MiddleBlock.Attribute.Color;
            }
        }
    }
}
