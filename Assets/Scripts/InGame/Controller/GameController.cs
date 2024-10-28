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

                // allocate 4 bit (0x000000F0)
                // game play state
                Processing_UserInput = 0x00000010,
                Done_GameEnd = 0x00000020,

                // allocate 8 bit (0x0000FF00)
                // game effect state
                GenerateEffect = 0x00000100,
                FairyEffect = 0x00000200,
                FallEffect = 0x00000400,
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
                    UIController.Instance.SetMoveText(_move.ToString());
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
                    _bossNowHealth = Mathf.Max(0, value);
                    UIController.Instance.BossHealthSet((float)_bossNowHealth / _bossMaxHealth);
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

            #region Etc object

            [Header("Etc object")]
            [SerializeField]
            private Transform _trHole;
            public Transform TrHole => _trHole;

            [SerializeField]
            private Transform _trBoss;
            public Transform TrBoss => _trBoss;

            [SerializeField]
            private int _runningFallObject;
            public int RunningFallObject
            {
                get { return _runningFallObject; }
                set
                {
                    if (_runningFallObject != value)
                    {
                        if (_runningFallObject == 0)
                        {
                            AddGameState(GameState.FallEffect);
                        }
                        _runningFallObject = value;

                        if (_runningFallObject == 0)
                        {
                            RemoveGameState(GameState.FallEffect);
                        }
                    }
                }
            }

            #endregion

            #region Data load

            private void LoadLevelData(int level)
            {
                TextAsset levelFile = Resources.Load<TextAsset>($"LevelData/{level}");
                if (levelFile == null)
                {
                    return;
                }
                JsonData root = JsonMapper.ToObject(levelFile.text);

                // Level type load
                _type = (LevelType)InGameUtils.ParseInt(ref root, ConstantData.LEVEL_DATA_LEVEL_TYPE);

                // Move load
                Move = InGameUtils.ParseInt(ref root, ConstantData.LEVEL_DATA_MOVE);

                // Star scores load
                if (root.ContainsKey(ConstantData.LEVEL_DATA_STAR_SCORES))
                {
                    LitJson.JsonData starScores = root[ConstantData.LEVEL_DATA_STAR_SCORES];
                    for (int i = 0; i < starScores.Count; ++i)
                    {
                        if (int.TryParse(starScores[i].ToString(), out int score))
                        {
                            _starScores[i + 1] = score;
                        }
                    }
                }

                // mission load
                if (root.ContainsKey(ConstantData.LEVEL_DATA_MISSION))
                {
                    // TODO
                }

                // boss health load
                _bossMaxHealth = _bossNowHealth = InGameUtils.ParseInt(ref root, ConstantData.LEVEL_DATA_BOSS_HEALTH);

                LitJson.JsonData mapRoot = root[ConstantData.LEVEL_DATA_MAP];
                _trMap.transform.position = InGameUtils.GetMapPosition();
                _map.LoadMapData(mapRoot);


                RemoveGameState(GameState.GameStart);
                bool isNeed = Map.IsNeedGenerateBlock();
                if (isNeed)
                {
                    AddGameState(GameState.GenerateEffect);
                    StartCoroutine(CheckGenerateBlock());
                }
            }

            private IEnumerator CheckGenerateBlock()
            {
                yield return new WaitForSeconds(1);

                Map.RequsetGenerate();
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
                Application.targetFrameRate = 60;
                Input.multiTouchEnabled = false;

                yield return null;

                UIController.Instance.Sponer.Init();
                LoadLevelData(10);
            }

            public System.Action IdleAction = null;

            public void RunIdleAction()
            {
                IdleAction?.Invoke();
                IdleAction = null;
            }

            #endregion

            #region Touch

            [Header("Touch")]
            [SerializeField]
            private List<HintLine> _hintLines = new List<HintLine>();

            [SerializeField]
            private List<HintLineInfo> _hintLineInfo = new List<HintLineInfo>();


            private Cell _previouseCell;

            private void InputTouch(Vector2 touchPosition, TouchPhase type)
            {
                if (State != GameState.Idle)
                {
                    TouchCancle();
                    return;
                }
                if(_bossNowHealth == 0)
                {
                    TouchCancle();
                    return;
                }

                switch (type)
                {
                    case TouchPhase.Began:
                    case TouchPhase.Moved:
                    {
                        OnTouchDrag(touchPosition);
                        break;
                    }
                    case TouchPhase.Ended:
                    {
                        OnTouchUp(touchPosition);
                        break;
                    }
                    case TouchPhase.Canceled:
                    {
                        TouchCancle();
                        break;
                    }
                }
            }

            private void TouchCancle()
            {
                if (_previouseCell != null)
                {
                    _previouseCell.SetHintEnable(false);
                    _previouseCell = null;
                }
                for(int i = 0; i < _hintLines.Count; i++)
                {
                    _hintLines[i].OffHint();
                }
                UIController.Instance.BossUIAlphaSet(1.0f);
                _hintLineInfo.Clear();
            }

            public void OnTouchUp(Vector2 touchPosition)
            {
                if(_previouseCell == null)
                {
                    return;
                }
                List<Vector3> targetPositions = new List<Vector3>();
                for(int i = 0; i < _hintLineInfo.Count; ++i)
                {
                    targetPositions.Add(_hintLineInfo[i].Target);
                }
                --Move;
                BlockType popCircleType = UIController.Instance.Sponer.PopCircle();
                _previouseCell.Block.ShootBlock(popCircleType, targetPositions);
                if (popCircleType != BlockType.BigBombCircle)
                {
                    UIController.Instance.Sponer.AddDefaultCircle();
                }
                AddGameState(GameState.Processing_UserInput);
                TouchCancle();
            }

            private struct HintLineInfo
            {
                public float Degree;
                public Vector3 Pivot;
                public Vector3 Target;
            }
            public void OnTouchDrag(Vector2 touchPosition)
            {
                TouchCancle();
                Vector2 sponerPosition = UIController.Instance.PosMainSponer;
                Vector2 targetPosition = touchPosition - sponerPosition;
                float fDegree = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
                int degree = Mathf.RoundToInt(fDegree * 100f);
                if (fDegree < ConstantData.Degree_LIMIT || fDegree > 180 - ConstantData.Degree_LIMIT)
                {
                    TouchCancle();
                    return;
                }
                List<int> detectedIndex = new List<int>();
                if (Map.IsStartOdd)
                {
                    detectedIndex = GameManager.Instance.OddDetect[degree];
                }
                else
                {
                    detectedIndex = GameManager.Instance.EvenDetect[degree];
                }

                int previouseY = -1;
                List<Cell> previouseEmptyCellList = new List<Cell>();
                List<Cell> emptyCellList = new List<Cell>();
                for (int i = 0; i < detectedIndex.Count; ++i)
                {
                    Cell cell = Map.GetCell(detectedIndex[i]);
                    if (cell.Type == CellType.None)
                    {
                        previouseEmptyCellList = new List<Cell>(emptyCellList);
                        break;
                    }
                    if (cell.Block.IsEmpty)
                    {
                        int y = detectedIndex[i] / ConstantData.MAX_WIDTH_NUM;
                        if (y != previouseY)
                        {
                            previouseY = y;
                            previouseEmptyCellList = new List<Cell>(emptyCellList);
                            emptyCellList.Clear();
                        }
                        emptyCellList.Add(cell);
                    }
                    else
                    {
                        previouseEmptyCellList = new List<Cell>(emptyCellList);
                        break;
                    }
                }
                Cell targetCell = null;
                if (previouseEmptyCellList.Count != 0 && degree != 9000)
                {
                    List<HintLineInfo> hintLineInfos = new List<HintLineInfo>();
                    List<LineEquationInfo> lineEquationInfos = new List<LineEquationInfo>();
                    float delta = sponerPosition.y;
                    float targetDegree = fDegree;
                    Vector2 targetPos = sponerPosition;
                    float minDistance = float.MaxValue;
                    Vector2 previousPos = targetPos;
                    float previousDegree = targetDegree;
                    while (delta <= previouseEmptyCellList[0].transform.position.y)
                    {
                        previousPos = targetPos;
                        previousDegree = targetDegree;
                        float gradient = InGameUtils.ComputeGradient(targetDegree);
                        LineEquationInfo lineEquationInfo = new LineEquationInfo()
                        {
                            Gradient = gradient,
                            Delta = InGameUtils.ComputeDelta(gradient, targetPos),
                            IsPositiveDirection = targetDegree < 90
                        };
                        lineEquationInfos.Add(lineEquationInfo);

                        if (targetDegree > 90)
                        {
                            targetDegree = 180 - targetDegree;
                            delta = InGameUtils.ComputeLineEquation(lineEquationInfo, -5);
                            targetPos = new Vector2(-5, delta);
                        }
                        else
                        {
                            targetDegree = targetDegree + 90;
                            delta = InGameUtils.ComputeLineEquation(lineEquationInfo, 5);
                            targetPos = new Vector2(5, delta);
                        }
                        if(delta <= previouseEmptyCellList[0].transform.position.y)
                        {
                            hintLineInfos.Add(new HintLineInfo()
                            {
                                Pivot = previousPos,
                                Target = targetPos,
                                Degree = previousDegree,
                            });
                        }
                    }
                    for (int i = 0; i < lineEquationInfos.Count; ++i)
                    {
                        LineEquationInfo lineEquationInfo = lineEquationInfos[i];
                        for (int j = 0; j < previouseEmptyCellList.Count; ++j)
                        {
                            float distance = InGameUtils.ComputeDistance(lineEquationInfo, previouseEmptyCellList[j].transform.position);
                            if (distance <= ConstantData.CIRCLE_RADIUS * 2)
                            {
                                if(distance < minDistance)
                                {
                                    minDistance = distance;
                                    targetCell = previouseEmptyCellList[j];
                                }
                            }
                        }
                    }
                    if (targetCell != null)
                    {
                        hintLineInfos.Add(new HintLineInfo()
                        {
                            Pivot = previousPos,
                            Target = targetCell.transform.position,
                            Degree = previousDegree,
                        });

                        _hintLineInfo = new List<HintLineInfo>(hintLineInfos);
                        float totalDistance = 0;
                        for (int i = 0; i < hintLineInfos.Count; ++i)
                        {
                            bool isLimit = false;
                            float distance = (hintLineInfos[i].Target - hintLineInfos[i].Pivot).magnitude;
                            if (totalDistance + distance > ConstantData.HINT_LINE_LIMIT)
                            {
                                distance = ConstantData.HINT_LINE_LIMIT - totalDistance;
                                isLimit = true;
                            }
                            else
                            {
                                totalDistance += distance;
                            }
                            _hintLines[i].SetHint(UIController.Instance.Sponer.PeekCircle(), hintLineInfos[i].Pivot, distance, hintLineInfos[i].Degree - 90);
                            if (isLimit)
                            {
                                break;
                            }
                        }
                    }
                }
                else if(previouseEmptyCellList.Count != 0)
                {
                    targetCell = previouseEmptyCellList[0];
                    _hintLineInfo.Add(new HintLineInfo() { Target = sponerPosition});
                    _hintLines[0].SetHint(UIController.Instance.Sponer.PeekCircle(), sponerPosition, (previouseEmptyCellList[0].transform.position - (Vector3)sponerPosition).magnitude, 0);
                }
                if (targetCell != null)
                {
                    UIController.Instance.BossUIAlphaSet(0.5f);
                    _previouseCell = targetCell;
                    _previouseCell.SetHintEnable(true);
                }
                else
                {
                    TouchCancle();
                }
            }

            public void OnUsetInput()
            {
                AddGameState(GameState.Processing_UserInput);
            }

            #endregion

        }
    }
}
