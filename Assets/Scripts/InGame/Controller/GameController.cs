using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
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
