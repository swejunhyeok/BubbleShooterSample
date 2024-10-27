using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class HintLine : MonoBehaviour
        {

            [SerializeField]
            private SpriteRenderer _srRenderer;

            [SerializeField]
            private Transform _trParent;

            [SerializeField]
            private Transform _trChild;

            public void SetHint(BlockType type, Vector2 pivotPoisition, float size, float degree)
            {
                _srRenderer.enabled = true;
                switch(type)
                {
                    case BlockType.RedCircle:
                    {
                        _srRenderer.color = Color.red;
                        break;
                    }
                    case BlockType.YellowCircle:
                    {
                        _srRenderer.color = Color.yellow;
                        break;
                    }
                    case BlockType.BlueCircle:
                    {
                        _srRenderer.color = Color.blue;
                        break;
                    }
                    default:
                    {
                        _srRenderer.color = Color.white;
                        break;
                    }
                }
                _trParent.transform.position = pivotPoisition;
                _trParent.rotation = Quaternion.Euler(0, 0, degree);
                _trChild.transform.localScale = new Vector2(0.1f, size);
                _trChild.transform.localPosition = new Vector2(0, size/2);
            }

            public void OffHint()
            {
                _srRenderer.enabled = false;
            }

        }
    }
}
