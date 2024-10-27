using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public enum LevelType
        {
            Normal,
            Boss,
        }
        public class GameController : SingletonController<GameController>
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

            #region Game state

            [System.Flags]
            public enum GameState
            {
                Idle = 0x00000000,

                // allocate 4bit (0x0000000F)
                // game initalize state
                GameStart = 0x00000001,
                GameEffect = 0x00000002,

                // allocate 4 bit (0x000000F0)
                // game play state
                Processing_UserInput = 0x00000010,
                Done_GameEnd = 0x00000020,
            }

            [SerializeField]
            private GameState _state = GameState.GameStart;
            public GameState State => _state;

            public void AddGameState(GameState state)
            {
                _state |= state;
            }

            public void RemoveGameState(GameState state)
            {
                _state &= ~state;
            }

            public bool IsContainState(GameState state)
            {
                return (_state & state) == state;
            }

            #endregion

            #region Level

            [Header("Level")]
            [SerializeField]
            private LevelType _type;
            public LevelType Type => _type;

            [SerializeField]
            private int _move;
            public int Move
            {
                get => _move;
                set
                {
                    _move = value;
                }
            }

            [SerializeField]
            private int[] _starScores = new int[4];
            public int[] StarScores => _starScores;

            [SerializeField]
            private int _bossMaxHealth = 0;
            public int BossMaxHealth => _bossMaxHealth;

            [SerializeField]
            private int _bossNowHealth = 0;
            public int BossNowHealth
            {
                get => _bossNowHealth;
                set
                {
                    _bossNowHealth = value;
                }
            }

            #endregion

            #region Map

            [Header("Map")]
            [SerializeField]
            private Transform _trMap;
            public Transform TrMap => _trMap;

            [SerializeField]
            private Map _map;
            public Map Map => _map;

            #endregion

            #region Data load

            private void LoadLevelData(int level)
            {
                TextAsset levelFile = Resources.Load<TextAsset>($"LevelData/{level}");
                if(levelFile == null)
                {
                    return;
                }
                JsonData root = JsonMapper.ToObject(levelFile.text);

                // Move load
                Move = InGameUtils.ParseInt(ref root, ConstantData.LEVEL_DATA_MOVE);

                // Star scores load
                if (root.ContainsKey(ConstantData.LEVEL_DATA_STAR_SCORES))
                {
                    LitJson.JsonData starScores = root[ConstantData.LEVEL_DATA_STAR_SCORES];
                    for(int i = 0; i < starScores.Count; ++i)
                    {
                        if (int.TryParse(starScores[i].ToString(), out int score))
                        {
                            _starScores[i + 1] = score;
                        }
                    }
                }

                // mission load
                if(root.ContainsKey(ConstantData.LEVEL_DATA_MISSION))
                {
                    // TODO
                }

                // boss health load
                _bossMaxHealth = _bossNowHealth = InGameUtils.ParseInt(ref root, ConstantData.LEVEL_DATA_BOSS_HEALTH);

                LitJson.JsonData mapRoot = root[ConstantData.LEVEL_DATA_MAP];
                _trMap.transform.position = InGameUtils.GetMapPosition();
                _map.LoadMapData(mapRoot);
            }

            #endregion

            #region General

            private void Start()
            {
                InputController.Instance.SetTouchAction(InputTouch);
                StartCoroutine(GameStart());
            }

            IEnumerator GameStart()
            {
                yield return null;

                Application.targetFrameRate = 120;
                Input.multiTouchEnabled = false;

                LoadLevelData(10);
            }

            #endregion

            #region Touch

            private Vector2Int _targetCellPos = CellIndex.None;
            private Vector2 _touchDownPoisition;

            private void InputTouch(Vector2 touchPosition, TouchPhase type)
            {
                Vector2Int cellPosition = Vector2Int.zero;
                if (cellPosition == CellIndex.None)
                {
                    _targetCellPos = CellIndex.None;
                    return;
                }

                if (IsContainState(GameState.Processing_UserInput) ||
                    IsContainState(GameState.GameEffect) ||
                    IsContainState(GameState.GameStart) ||
                    IsContainState(GameState.Done_GameEnd))
                {
                    _targetCellPos = CellIndex.None;
                    return;
                }

                switch (type)
                {
                    case TouchPhase.Began:
                    {
                        OnTouchDown(cellPosition, touchPosition);
                        break;
                    }
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                    {
                        OnTouchUp(cellPosition, touchPosition);
                        break;
                    }
                    case TouchPhase.Moved:
                    {
                        OnTouchDrag(cellPosition, touchPosition);
                        break;
                    }
                }
            }

            public void OnTouchDown(Vector2Int cellPosition, Vector2 touchPosition)
            {
                _targetCellPos = cellPosition;
                _touchDownPoisition = touchPosition;
            }

            public void OnTouchUp(Vector2Int cellPosition, Vector2 touchPosition)
            {
                if (_targetCellPos == CellIndex.None)
                {
                    return;
                }
                if (_targetCellPos != cellPosition)
                {
                    OnTouchDrag(cellPosition, touchPosition);
                    return;
                }

                //OnUsetInput();
                _targetCellPos = CellIndex.None;
            }

            public void OnTouchDrag(Vector2Int cellPosition, Vector2 touchPosition)
            {
                if (_targetCellPos == CellIndex.None)
                {
                    return;
                }
                if (_targetCellPos == cellPosition)
                {
                    return;
                }

                Vector2 diffPosition = touchPosition - _touchDownPoisition;
                bool isPositiveX = diffPosition.x > 0;
                bool isPositiveY = diffPosition.y > 0;
                float absX = isPositiveX ? diffPosition.x : -diffPosition.x;
                float absY = isPositiveY ? diffPosition.y : -diffPosition.y;

                OnUsetInput();
                _targetCellPos = CellIndex.None;
            }

            public void OnUsetInput()
            {
                AddGameState(GameState.Processing_UserInput);
            }

            #endregion

        }
    }
}
