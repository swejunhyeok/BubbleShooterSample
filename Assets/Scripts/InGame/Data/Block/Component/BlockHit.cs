using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
                    Fairy fairy = ObjectPoolController.Instance.GetFairy();
                    fairy.RunFairy(transform.position, GameController.Instance.TrBoss.position, () => { --GameController.Instance.BossNowHealth; });
                }
                if (isDestroy)
                {
                    ScoreText scoreText = ObjectPoolController.Instance.GetScoreText();
                    int score = 0;
                    if(hitCondition != HitConditionType.SpecialBlock && hitBlock != BlockType.BigBombCircle)
                    {
                        score = ConstantData.BIG_BOMB_DESTROY_SCORE;
                    }
                    else
                    {
                        score = Parent.Attribute.HitScore;
                        if(Parent.Attribute.IsHitScoreMultiplyComb)
                        {
                            score *= GameController.Instance.Comb;
                        }
                    }
                    GameController.Instance.Score += score;
                    scoreText.SetScoreText(score.ToString(), transform.position);
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
                ++GameController.Instance.RunningFallObject;
                Parent.State.SetState(BlockStateType.Destroy);
                Parent.RemovePivotCell();
                Parent.Move.SetMoveEndAction(() =>
                {
                    MoveEndFunc(Parent.Attribute.FallScore, Parent.Attribute.IsFallScoreMultiplyComb);
                });
                Parent.Move.Move(BlockMove.MoveType.JustMove, GameController.Instance.TrHole.position);
            }

            private void MoveEndFunc(int score, bool isMultiPly)
            {
                --GameController.Instance.RunningFallObject;
                ScoreText scoreText = ObjectPoolController.Instance.GetScoreText();
                if (isMultiPly)
                {
                    score *= GameController.Instance.Comb;
                }
                GameController.Instance.Score += score;
                scoreText.SetScoreText(score.ToString(), transform.position);
                Parent.Dispose();
            }

            public void RetainMoveDestroy(int score, System.Action arriveAction)
            {
                Parent.State.SetState(BlockStateType.Destroy);
                Parent.RemovePivotCell();

                Parent.Move.SetMoveEndAction(() =>
                {
                    MoveEndFunc(score, false);
                    arriveAction();
                });
                Parent.Move.Move(BlockMove.MoveType.JustMove, Vector3.zero);
            }

            public void RetainBlockDestroy()
            {
                ScoreText scoreText = ObjectPoolController.Instance.GetScoreText();
                int score = Parent.Attribute.RetainScore;
                GameController.Instance.Score += score;
                scoreText.SetScoreText(score.ToString(), transform.position);

                Parent.RemovePivotCell();
                Parent.Dispose();
            }

            #endregion
        }
    }
}
