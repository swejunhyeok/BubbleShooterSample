using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class ScoreText : BBData
        {
            [SerializeField]
            private Transform _trMain;

            [SerializeField]
            private TextMesh _tmScore;

            [SerializeField]
            private MeshRenderer _mrScore;

            [SerializeField]
            private float _duration = 0.5f;

            [SerializeField]
            private float _upPosY = 1.5f;

            public void SetScoreText(string score, Vector3 startPosition)
            {
                _mrScore.sortingLayerID = SortingLayer.NameToID("Cover");
                _tmScore.text = score;
                _trMain.position = startPosition;
                StartCoroutine(TextMoving());
            }

            private IEnumerator TextMoving()
            {
                float timeDepth = InGameUtils.GetTimeDepth(_duration);
                float delta = 0;
                Vector3 orgPosition = _trMain.position;
                while(delta < 1)
                {
                    delta += Time.deltaTime * timeDepth;
                    _trMain.position = Vector3.Lerp(orgPosition, orgPosition + new Vector3(0, _upPosY, 0), delta);
                    if(delta < 1)
                    {
                        yield return null;
                    }
                }
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
