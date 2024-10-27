using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JH
{
    namespace BBS
    {
        public class SponerCircle : MonoBehaviour
        {
            #region UI

            [Header("UI")]
            [SerializeField]
            private Image _imgSponerCircle;

            [System.Serializable]
            private struct CircleSpriteInfo
            {
                public BlockType Type;
                public Sprite SprCircle;
            }

            [SerializeField]
            private CircleSpriteInfo[] _sprCircles;

            public void SetCircle(BlockType type)
            {
                for (int i = 0; i < _sprCircles.Length; ++i)
                {
                    if (_sprCircles[i].Type == type)
                    {
                        _imgSponerCircle.sprite = _sprCircles[i].SprCircle;
                    }
                }
            }

            #endregion
        }
    }
}
