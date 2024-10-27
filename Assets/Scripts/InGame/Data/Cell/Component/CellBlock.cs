using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class CellBlock : CellComponent
        {

            #region Cell

            [Header("Cell")]
            [SerializeField]
            private Transform _trCell;

            #endregion

            #region Block

            [Header("Block")]
            [SerializeField]
            private Block _bottomBlock;
            public Block BottomBlock => _bottomBlock;
            public bool HasBottomBlock => BottomBlock != null;

            [SerializeField]
            private Block _middleBlock;
            public Block MiddleBlock => _middleBlock;
            public bool HasMiddleBlock => MiddleBlock != null;

            [SerializeField]
            private Block _topBlock;
            public Block TopBlock => _topBlock;
            public bool HasTopBlock => TopBlock != null;

            public bool IsEmpty
            {
                get
                {
                    if(HasBottomBlock)
                    {
                        return false;
                    }
                    if(HasMiddleBlock)
                    {
                        return false;
                    }
                    if(HasTopBlock)
                    {
                        return false;
                    }
                    return true;
                }
            }

            #endregion

            #region Block manage

            public void CreateBlock(BlockType type)
            {
                Block block = ObjectPoolController.Instance.GetBlock(_trCell);

                block.SetAttribute(type);

                AddBlock(block);
            }

            public bool AddBlock(Block block, bool isResetPosition = true)
            {
                if(block == null)
                {
                    return false;
                }
                bool isAddSuccess = AddLayerBlock(block);
                if(!isAddSuccess)
                {
                    return false;
                }
                block.ChangePivotCell(Parent, isResetPosition);
                return isAddSuccess;
            }

            public bool AddLayerBlock(Block block)
            {
                if(block == null)
                {
                    return false;
                }
                if (!block.HasAttribute)
                {
                    return false;
                }
                if(block.Attribute.IsBottomLayer)
                {
                    if(HasBottomBlock)
                    {
                        return false;
                    }
                    _bottomBlock = block;
                }
                if(block.Attribute.IsMiddleLayer)
                {
                    if(HasMiddleBlock)
                    {
                        return false;
                    }
                    _middleBlock = block;
                }
                if(block.Attribute.IsTopLayer)
                {
                    if(HasTopBlock)
                    {
                        return false;
                    }
                    _topBlock = block;
                }
                return true;
            }

            public void PopBlock(Block block)
            {
                if(block == null)
                {
                    return;
                }
                if(BottomBlock == block)
                {
                    _bottomBlock = null;
                }
                if(MiddleBlock == block)
                {
                    _middleBlock = null;
                }
                if(TopBlock == block)
                {
                    _topBlock = null;
                }
            }

            #endregion

            #region General

            public override void Init()
            {
                base.Init();
                _bottomBlock = null;
                _middleBlock = null;
                _topBlock = null;
            }

            public override void Dispose()
            {
                base.Dispose();
                _bottomBlock = null;
                _middleBlock = null;
                _topBlock = null;
            }

            #endregion

        }
    }
}
