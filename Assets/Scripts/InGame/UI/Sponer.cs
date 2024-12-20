using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class Sponer : MonoBehaviour
        {

            #region UI

            [Header("UI")]
            [SerializeField]
            private SponerCircle[] _sponerCircles;

            [SerializeField]
            private List<BlockType> _sponerInfo = new List<BlockType>();

            [SerializeField]
            private List<BlockType> _defaultBlockType = new List<BlockType>();

            public void Init()
            {
                for (int i = 0; i < 2; ++i)
                {
                    BlockType selectType = _defaultBlockType[Random.Range(0, _defaultBlockType.Count)];
                    _sponerInfo.Add(selectType);
                    _sponerCircles[i].SetCircle(selectType);
                }
            }

            public void AddBigBombCircle()
            {
                _sponerCircles[2].gameObject.SetActive(true);
                _sponerInfo.Insert(0, BlockType.BigBombCircle);
                SetCircleImage();
            }

            public void UseBigBombCircle()
            {
                _sponerCircles[2].gameObject.SetActive(false);

                UIController.Instance.SetNerosBeedsActive(true);
            }

            public void AddDefaultCircle()
            {
                if(GameController.Instance.Move == _sponerInfo.Count)
                {
                    return;
                }
                _sponerInfo.Add(_defaultBlockType[Random.Range(0, _defaultBlockType.Count)]);
                SetCircleImage();
            }

            private void SetCircleImage()
            {
                for (int i = 0; i < _sponerInfo.Count; ++i)
                {
                    _sponerCircles[i].SetCircle(_sponerInfo[i]);
                    _sponerCircles[i].gameObject.SetActive(true);
                }
                for(int i = _sponerInfo.Count; i < _sponerCircles.Length; ++i)
                {
                    _sponerCircles[i].gameObject.SetActive(false);
                }
            }

            public BlockType PeekCircle()
            {
                return _sponerInfo[0];
            }

            public BlockType PopCircle()
            {
                BlockType firstType = _sponerInfo[0];
                _sponerInfo.RemoveAt(0);

                if(firstType == BlockType.BigBombCircle)
                {
                    UseBigBombCircle();
                }

                SetCircleImage();

                return firstType;
            }

            public void ChangeCircle()
            {
                if (GameController.Instance.State != GameController.GameState.Idle)
                {
                    return;
                }
                BlockType firstType = _sponerInfo[0];
                _sponerInfo.RemoveAt(0);
                _sponerInfo.Add(firstType);

                SetCircleImage();
            }

            #endregion

        }
    }
}
