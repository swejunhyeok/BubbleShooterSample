using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class BlockMove : BlockComponent
        {

            #region Move

            public enum MoveType
            {
                BlockFill,
                BlockShoot,
            }

            private System.Action _moveEndAction;

            public void SetMoveEndAction(System.Action action)
            {
                _moveEndAction += action;
            }

            public void Move(MoveType type)
            {
                OnMoveStart(type);
                StartCoroutine(MoveLocalPositionZero(type));
            }

            public void Move(MoveType type, Vector3 targetPosition)
            {
                OnMoveStart(type);
                StartCoroutine(MoveSpecificPosition(type, targetPosition));
            }

            public void Move(MoveType type, List<Vector3> targetPositions)
            {
                OnMoveStart(type);
                StartCoroutine(MoveMultiPositions(type, targetPositions));
            }

            protected IEnumerator MoveLocalPositionZero(MoveType type, float duration = 0.5f)
            {
                float timeDepth = InGameUtils.GetTimeDepth(duration);
                float deltaSum = 0f;
                Vector2 orgPosition = transform.localPosition;
                while (deltaSum < 1f)
                {
                    deltaSum += timeDepth * Time.deltaTime;
                    transform.localPosition = Vector2.Lerp(orgPosition, Vector2.zero, deltaSum);
                    if (deltaSum >= 1f)
                    {
                        break;
                    }
                    yield return null;
                }
                transform.localPosition = Vector3.zero;
                OnMoveEnd(type);
            }


            protected IEnumerator MoveSpecificPosition(MoveType type, Vector3 targetPosition, float duration = 0.25f)
            {
                float timeDepth = InGameUtils.GetTimeDepth(duration);
                float deltaSum = 0f;
                Vector3 orgPosition = transform.position;
                while (deltaSum < 1f)
                {
                    deltaSum += timeDepth * Time.deltaTime;
                    transform.position = Vector3.Lerp(orgPosition, targetPosition, deltaSum);
                    if (deltaSum >= 1f)
                    {
                        break;
                    }
                    yield return null;
                }
                transform.position = targetPosition;
                OnMoveEnd(type);
            }

            protected IEnumerator MoveMultiPositions(MoveType type, List<Vector3> targetPositions, float duration = 0.25f)
            {
                List<float> timeDepth = new List<float>();
                
                List<float> distances = new List<float>();
                float totalDistance = 0;
                Vector3 previousPosition = transform.position;
                for(int i = 0; i < targetPositions.Count; ++i)
                {
                    float distance = (targetPositions[i] - previousPosition).magnitude;
                    distances.Add(distance);
                    totalDistance += distance;
                    previousPosition = targetPositions[i];
                }

                for(int i = 0; i < distances.Count; ++i)
                {
                    timeDepth.Add(InGameUtils.GetTimeDepth((distances[i] / totalDistance) * duration));
                }

                previousPosition = transform.position;
                for (int i = 0; i < timeDepth.Count; ++i)
                {
                    float deltaSum = 0f;
                    while (deltaSum < 1f)
                    {
                        deltaSum += timeDepth[i] * Time.deltaTime;
                        transform.position = Vector3.Lerp(previousPosition, targetPositions[i], deltaSum);
                        if (deltaSum >= 1f)
                        {
                            break;
                        }
                        yield return null;
                    }
                    previousPosition = targetPositions[i];
                }

                transform.position = targetPositions[targetPositions.Count - 1];
                OnMoveEnd(type);
            }

            #endregion

            #region Move start func

            protected void OnMoveStart(MoveType type)
            {
                CommonMoveStart();
                switch(type)
                {
                    case MoveType.BlockFill:
                    {
                        OnBlockFillStart();
                        break;
                    }
                    case MoveType.BlockShoot:
                    {
                        OnBlockShootStart();
                        break;
                    }
                }
            }

            protected void CommonMoveStart()
            {
                Parent.State.SetState(BlockStateType.Move);
            }

            protected virtual void OnBlockFillStart() { }
            protected virtual void OnBlockShootStart() { }
            
            #endregion

            #region Move end func

            protected void OnMoveEnd(MoveType type)
            {
                CommonMoveEnd();
                switch(type)
                {
                    case MoveType.BlockFill:
                    {
                        OnBlockFillEnd();
                        break;
                    }
                    case MoveType.BlockShoot:
                    {
                        OnBlockShootEnd();
                        break;
                    }
                }
            }

            protected void CommonMoveEnd()
            {
                if(Parent.State.State != BlockStateType.Destroy && Parent.State.State != BlockStateType.Dispose)
                {
                    Parent.State.SetState(BlockStateType.Idle);
                }
                _moveEndAction?.Invoke();
                _moveEndAction = null;
            }

            protected virtual void OnBlockFillEnd() { }
            protected virtual void OnBlockShootEnd() { }

            #endregion

            #region General

            public override void Init()
            {
                base.Init();
                _moveEndAction = null;
                StopAllCoroutines();
            }

            public override void Dispose()
            {
                base.Dispose();
                _moveEndAction = null;
                StopAllCoroutines();
            }

            #endregion
        }
    }
}
