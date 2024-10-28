using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JH
{
    namespace BBS
    {
        public class UIController : SingletonController<UIController>
        {

            #region Instance

            void Awake()
            {
                if (_instance == null)
                {
                    _instance = this;
                }

                if (_instance != this)
                {
                    Destroy(gameObject);
                }
            }

            #endregion

            #region Unity component

            [Header("Unity component")]
            [SerializeField]
            private Camera _cameraMain;

            [SerializeField]
            private RectTransform _trMainSponer;

            private Vector2 _posMainSponer = Vector2.one * int.MinValue;
            public Vector2 PosMainSponer
            {
                get
                {
                    if (_posMainSponer == Vector2.one * int.MinValue)
                    {
                        _posMainSponer = _cameraMain.ScreenToWorldPoint(_trMainSponer.transform.position);
                    }
                    return _posMainSponer;
                }
            }

            #endregion

            #region UI

            [Header("UI")]
            [SerializeField]
            private Image[] _imgBossUIs;

            [SerializeField]
            private Text _txtMove;

            [SerializeField]
            private Image _imgBossHealth;

            [SerializeField]
            private GameObject _goNerosBeeds;
            [SerializeField]
            private Image _imgNerosBeeds;

            [SerializeField]
            private Sponer _sponer;
            public Sponer Sponer => _sponer;

            private int _nowNerosBeedsCount = 0;

            public void SetMoveText(string move) => _txtMove.text = move;

            public void BossHealthSet(float amount) => _imgBossHealth.fillAmount = amount;

            public void BossUIAlphaSet(float alpha)
            {
                for(int i = 0; i < _imgBossUIs.Length; ++i)
                {
                    _imgBossUIs[i].color = new Color(1, 1, 1, alpha);
                }
            }

            public void SetNerosBeedsActive(bool isActive)
            {
                _goNerosBeeds.SetActive(isActive);
                if(isActive)
                {
                    _imgNerosBeeds.fillAmount = 0;
                    _nowNerosBeedsCount = 0;
                }
            }

            public void AddNerosBeedsCount()
            {
                ++_nowNerosBeedsCount;
                _imgNerosBeeds.fillAmount = (float)_nowNerosBeedsCount / ConstantData.NEROS_BEEDS_NUM;
                if(_nowNerosBeedsCount >= ConstantData.NEROS_BEEDS_NUM)
                {
                    SetNerosBeedsActive(false);
                    Sponer.AddBigBombCircle();
                }
            }

            public void OnClickNerosBeeds()
            {
                if(GameController.Instance.State != GameController.GameState.Idle)
                {
                    return;
                }
                --GameController.Instance.Move;
                Sponer.PopCircle();
                AddNerosBeedsCount();
                Sponer.AddDefaultCircle();
            }

            #endregion

            #region Editor

#if UNITY_EDITOR

            [EditorButton("Debug sponer position", "DebugSponerPosition")]
            public void DebugSponerPosition()
            {
                Debug.Log(PosMainSponer.ToString());
            }

#endif

            #endregion

        }
    }
}
