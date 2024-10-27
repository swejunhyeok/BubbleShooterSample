using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

            #region Block component

            [Header("Block component")]
            [SerializeField]
            private BlockMatch _match;
            public BlockMatch Match => _match;

            [SerializeField]
            private BlockMove _move;
            public BlockMove Move => _move;

            [SerializeField]
            private BlockSprite _sprite;
            public BlockSprite Sprite => _sprite;

            [SerializeField]
            private BlockState _state;
            public BlockState State => _state;

            #endregion

            #region Block control

            public void SetAttribute(BlockType type)
            {
                SetAttribute(AddressableController.Instance.GetBlockAttribute(type));
            }

            public void SetAttribute(BlockAttribute blockAttribute)
            {
                _attribute = blockAttribute;
                Sprite.SetMainSprite(Attribute.SprBlock);
                Sprite.SetMainSpriteSorting();
                Sprite.SetFairyEnable((Attribute.HitEffect & HitEffectType.Fairy) == HitEffectType.Fairy);
            }

            public void ChangePivotCell(Cell cell, bool isResetPosition)
            {
                _pivotCell = cell;
                transform.parent = cell.transform;
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

            #region General

            public override void Init()
            {
                base.Init();

                _pivotCell = null;
                _attribute = null;

                if(Match != null)
                {
                    Match.Init();
                }
                if(Move != null)
                {
                    Move.Init();
                }
                if(Sprite != null)
                {
                    Sprite.Init();
                }
                if(State != null)
                {
                    State.Init();
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                if (Match != null)
                {
                    Match.Dispose();
                }
                if (Move != null)
                {
                    Move.Dispose();
                }
                if (Sprite != null)
                {
                    Sprite.Dispose();
                }
                if (State != null)
                {
                    State.Dispose();
                }
                ObjectPoolController.Instance.Dispose(this);
            }

            #endregion

            #region Editor

#if UNITY_EDITOR

            public virtual void Reset()
            {
                if(TryGetComponent<BlockMatch>(out var match))
                {
                    _match = match;
                }
                if(TryGetComponent<BlockMove>(out var move))
                {
                    _move = move;
                }
                if(TryGetComponent<BlockSprite>(out var sprite))
                {
                    _sprite = sprite;
                }
                if(TryGetComponent<BlockState>(out var state))
                {
                    _state = state;
                }
            }

#endif

            #endregion

        }
    }
}
