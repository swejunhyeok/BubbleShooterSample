using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class Block : BBData
        {

            #region Cell

            [Header("Cell")]
            [SerializeField]
            private Cell _pivotCell;
            public Cell PivotCell => _pivotCell;

            #endregion

            #region Block

            [Header("Block")]
            [SerializeField]
            private BlockAttribute _attribute;
            public BlockAttribute Attribute
            {
                get
                {
                    return _attribute;
                }
            }
            public bool HasAttribute => Attribute != null;

            #endregion

            #region Block control

            public void SetAttribute(BlockType type)
            {
                _attribute = AddressableController.Instance.GetBlockAttribute(type);
            }

            public void SetAttribute(BlockAttribute blockAttribute)
            {
                _attribute = blockAttribute;
            }

            public void ChangePivotCell(Cell cell, bool isResetPosition)
            {
                _pivotCell = cell;
                if(isResetPosition)
                {
                    transform.localPosition = Vector3.zero;
                }
            }

            public void RemovePivotCell()
            {
                PivotCell.Block.PopBlock(this);
                _pivotCell = null;
            }

            #endregion

        }
    }
}
