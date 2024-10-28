using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class Fairy : BBData
        {

            public void RunFairy(Vector2 startPosition, Vector2 endPosition, System.Action arriveAction)
            {
                transform.position = startPosition;
                StartCoroutine(FlyFairy(startPosition, endPosition, arriveAction));
            }

            public IEnumerator FlyFairy(Vector2 startPosition, Vector2 endPosition, System.Action arriveAction)
            {
                float timeDepth = InGameUtils.GetTimeDepth(0.5f);
                float delta = 0;
                while(delta < 1)
                {
                    delta += Time.deltaTime * timeDepth;
                    transform.position = Vector2.Lerp(startPosition, endPosition, delta);
                    if(delta < 1)
                    {
                        yield return null;
                    }
                }
                arriveAction?.Invoke();
                Dispose();
            }

            public override void Dispose()
            {
                base.Dispose();
                ObjectPoolController.Instance.Dispose(this);
            }

        }
    }
}
