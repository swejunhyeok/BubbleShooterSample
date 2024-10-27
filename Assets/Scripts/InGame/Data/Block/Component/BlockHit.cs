using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class BlockHit : BlockComponent
        {
            #region Hit

            protected bool CheckHitCondition(HitConditionType hitCondition)
            {
                if (!Parent.HasAttribute)
                {
                    return false;
                }
                return (Parent.Attribute.HitCondition & hitCondition) == hitCondition;
            }

            protected void RunHitEffect(HitConditionType hitCondition, BlockType hitBlock)
            {
                bool isDestroy = false;
                BlockAttribute changeAttribute = null;
                if ((Parent.Attribute.HitEffect & HitEffectType.Destroy) == HitEffectType.Destroy)
                {
                    isDestroy = true;
                }
                if(isDestroy)
                {
                    Parent.State.SetState(BlockStateType.Destroy);
                }
                if ((Parent.Attribute.HitEffect & HitEffectType.ChangeBlock) == HitEffectType.ChangeBlock)
                {
                    changeAttribute = Parent.Attribute.HitChangeBlock;
                }
                if ((Parent.Attribute.HitEffect & HitEffectType.Bomb) == HitEffectType.Bomb)
                {
                    Parent.PivotCell.Effect.RunBombEffect();
                }
                if((Parent.Attribute.HitEffect & HitEffectType.LargeBomb) == HitEffectType.LargeBomb)
                {
                    Parent.PivotCell.Effect.RunLargeBombEffect();
                }
                if((Parent.Attribute.HitEffect & HitEffectType.Fairy) == HitEffectType.Fairy)
                {
                    
                }
                if (isDestroy)
                {
                    Destroy();
                }
                if (changeAttribute != null)
                {
                    Parent.SetAttribute(changeAttribute);
                }
            }

            protected void ArroundHit(HitConditionType hitCondition, BlockType hitBlock)
            {
                LayerType hitLayer = LayerType.Bottom | LayerType.Middle | LayerType.Top;
                foreach (Cell cell in Parent.PivotCell.ArroundCell)
                {
                    if (cell == null)
                    {
                        continue;
                    }
                    if (Parent.Cache.HitPositions != null && Parent.Cache.HitPositions.IndexOf(cell.Pos) != -1)
                    {
                        continue;
                    }
                    cell.Block.Hit(hitLayer, Parent.Attribute.Type, HitConditionType.ArroundMatch, Parent.Cache.HitPositions);
                    if (Parent.Cache.HitPositions != null)
                    {
                        Parent.Cache.HitPositions.Add(cell.Pos);
                    }
                }
            }

            public void Hit(
                HitConditionType hitCondition,
                BlockType hitBlock,
                System.Action successCallback = null)
            {
                if(Parent.State.State == BlockStateType.Destroy || Parent.State.State == BlockStateType.Dispose)
                {
                    return;
                }
                if (!CheckHitCondition(hitCondition))
                {
                    return;
                }

                RunHitEffect(hitCondition, hitBlock);
                successCallback?.Invoke();
            }

            private void Destroy()
            {
                Parent.RemovePivotCell();
                Parent.Dispose();
            }

            public void FallBlock()
            {
                Parent.State.SetState(BlockStateType.Destroy);



                Parent.RemovePivotCell();
                Parent.Dispose();
            }


            #endregion
        }
    }
}
