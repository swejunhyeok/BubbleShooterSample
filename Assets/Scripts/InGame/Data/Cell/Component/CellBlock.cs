using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

            public Block HighestBlock
            {
                get
                {
                    if(HasTopBlock)
                    {
                        return TopBlock;
                    }
                    if(HasMiddleBlock)
                    {
                        return MiddleBlock;
                    }
                    if(HasBottomBlock)
                    {
                        return BottomBlock;
                    }
                    return null;
                }
            }

            #endregion

            #region Block manage

            public void ShootBlock(BlockType type, List<Vector3> targetPositions)
            {
                CreateBlock(type, false);
                MiddleBlock.transform.position = UIController.Instance.PosMainSponer;
                if (type == BlockType.BigBombCircle)
                {
                    ++GameController.Instance.Comb;
                    MiddleBlock.Move.SetMoveEndAction(() =>
                    {
                        MiddleBlock.Hit.Hit(HitConditionType.Arrive, BlockType.None);
                        GameController.Instance.Map.FallCheck();
                    });
                }
                else
                {
                    MiddleBlock.Move.SetMoveEndAction(() =>
                    {
                        BlockType type = MiddleBlock.Attribute.Type;
                        MiddleBlock.Match.RunMatch();
                        for (int i = 0; i < Parent.ArroundCell.Count; ++i)
                        {
                            if (Parent.ArroundCell[i] == null)
                            {
                                continue;
                            }
                            Parent.ArroundCell[i].Block.Hit(LayerType.Middle, type, HitConditionType.GetShot);
                        }
                        GameController.Instance.Map.FallCheck();
                    });
                }
                MiddleBlock.Move.Move(BlockMove.MoveType.BlockShoot, targetPositions);
            }

            public void CreateBlock(BlockType type, bool isResetPosition = true)
            {
                Block block = ObjectPoolController.Instance.GetBlock(_trCell);

                block.SetAttribute(type);

                AddBlock(block, isResetPosition);
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

            #region Hit

            public void Hit(LayerType layer, BlockType block, HitConditionType hitType, List<Vector2Int> hitPositions = null, System.Action successCallback = null)
            {
                if (HighestBlock == null)
                {
                    return;
                }
                if ((layer & LayerType.Top) == LayerType.Top)
                {
                    if (HighestBlock == TopBlock)
                    {
                        TopBlock.Cache.HitPositions = hitPositions;
                        TopBlock.Hit.Hit(hitType, block, successCallback);
                        return;
                    }
                }
                if ((layer & LayerType.Middle) == LayerType.Middle)
                {
                    if (HighestBlock == MiddleBlock)
                    {
                        MiddleBlock.Cache.HitPositions = hitPositions;
                        MiddleBlock.Hit.Hit(hitType, block, successCallback);
                        return;
                    }
                }
                if ((layer & LayerType.Bottom) == LayerType.Bottom)
                {
                    if (HighestBlock == BottomBlock)
                    {
                        BottomBlock.Cache.HitPositions = hitPositions;
                        BottomBlock.Hit.Hit(hitType, block, successCallback);
                        return;
                    }
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
