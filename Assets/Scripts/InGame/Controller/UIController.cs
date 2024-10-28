using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

            public void CameraMoveEffect()
            {
                GameController.Instance.AddGameState(GameController.GameState.CameraMove);
                _cameraMain.transform.position = new Vector3(0, 20, -10);
                StartCoroutine(CameraMoveing());
            }

            public IEnumerator CameraMoveing()
            {
                float timeDepth = InGameUtils.GetTimeDepth(0.5f);
                float delta = 0;
                Vector3 orgPosition = _cameraMain.transform.position;
                while(delta < 1)
                {
                    delta += Time.deltaTime * timeDepth;
                    _cameraMain.transform.position = Vector3.Lerp(orgPosition, new Vector3(0, 0, -10), delta);
                    if(delta < 1)
                    {
                        yield return null;
                    }
                }
                _cameraMain.transform.position = new Vector3(0, 0, -10);
                GameController.Instance.RemoveGameState(GameController.GameState.CameraMove);
                GameController.Instance.FinishCameraMove();
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

            [Header("Popup")]
            [SerializeField]
            private GameObject _goLoading;

            [SerializeField]
            private GameObject _goFail;

            [SerializeField]
            private GameObject _goClear;
            [SerializeField]
            private Text _txtClearScore;
            [SerializeField]
            private GameObject[] _goStars;

            public bool LoadingEnable => _goLoading.activeSelf;

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

            public void AddNerosBeedsCount(int increaseNum = 1)
            {
                if(!_goNerosBeeds.activeSelf)
                {
                    return;
                }
                _nowNerosBeedsCount += increaseNum;
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
                if(GameController.Instance.Move == 0)
                {
                    GameController.Instance.AddGameState(GameController.GameState.Done_GameFail);
                    SetFailPopup();
                }
                Sponer.PopCircle();
                AddNerosBeedsCount(ConstantData.NEROS_BEEDS_NUM / 4);
                Sponer.AddDefaultCircle();
            }

            public void LoadPlayScene()
            {
                SceneManager.LoadScene(ConstantData.PLAY_SCENE);
            }

            public void SetFailPopup()
            {
                _goFail.SetActive(true);
            }

            public void SetClearPopup(string score, int startNum)
            {
                _goClear.SetActive(true);
                _txtClearScore.text = "Score : " + score;
                for(int i = 0; i < startNum; ++i)
                {
                    _goStars[i].SetActive(true);
                }
                for(int i = startNum; i < _goStars.Length; ++i)
                {
                    _goStars[i].SetActive(false);
                }
            }

            #endregion

            #region General

            public void Start()
            {
                StartCoroutine(LoadingClose());
            }

            IEnumerator LoadingClose()
            {
                yield return new WaitForSeconds(1);
                // wait load someting.
                _goLoading.SetActive(false);
                GameController.Instance.RemoveGameState(GameController.GameState.Loading);
                CameraMoveEffect();
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
