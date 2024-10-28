using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class Map : BBData
        {

            #region Map

            [Header("Map")]
            [SerializeField]
            private bool _isStartOdd = false;
            public bool IsStartOdd => _isStartOdd;

            [SerializeField]
            private Transform _trMap;

            #endregion

            #region Cell

            [Header("Line")]
            [SerializeField]
            private List<Line> _lines = new List<Line>();

            public Cell GetCell(int index)
            {
                if(index < 0 || index / ConstantData.MAX_WIDTH_NUM >= _lines.Count)
                {
                    return null;
                }
                return _lines[index / ConstantData.MAX_WIDTH_NUM].GetCell(index % ConstantData.MAX_WIDTH_NUM);
            }

            public Cell GetCell(int x, int y)
            {
                if(x < 0 || x >= ConstantData.MAX_WIDTH_NUM)
                {
                    return null;
                }
                if(y < 0 || y >= _lines.Count)
                {
                    return null;
                }
                return _lines[y].GetCell(x);
            }

            public Cell GetCell(Vector2Int pos)
            {
                return GetCell(pos.x, pos.y);
            }

            #endregion

            #region Cell check

            [Header("Cell check")]
            [SerializeField]
            private List<Cell> _generateCell = new List<Cell>();
            [SerializeField]
            private List<Cell> _finishCell = new List<Cell>();

            public bool IsNeedGenerateBlock()
            {
                for(int i = 0; i < _generateCell.Count; ++i)
                {
                    Cell targetCell = _generateCell[i].GetArroundCell((int)_generateCell[i].Direction);
                    while(targetCell != null)
                    {
                        if(targetCell.Block.IsEmpty)
                        {
                            return true;
                        }
                        targetCell = targetCell.GetArroundCell((int)targetCell.Direction);
                    }
                }
                return false;
            }
            
            public bool CheckGenerateBlock()
            {
                for (int i = 0; i < _generateCell.Count; ++i)
                {
                    Cell targetCell = _generateCell[i].GetArroundCell((int)_generateCell[i].Direction);
                    while (targetCell != null)
                    {
                        if (targetCell.Block.IsEmpty)
                        {
                            return false;
                        }
                        if(targetCell.Block.MiddleBlock.State.State == BlockStateType.Move)
                        {
                            return false;
                        }
                        targetCell = targetCell.GetArroundCell((int)targetCell.Direction);
                    }
                }
                return true;
            }

            public void RequsetGenerate()
            {
                for(int i = 0; i < _generateCell.Count; ++i)
                {
                    _generateCell[i].Generate.GenerateObject();
                }
            }

            #endregion

            #region Fall check

            private class FallInfo
            {
                public bool IsArriveEnd = false;
                public List<Cell> ConnectedCell = new List<Cell>();
            }

            public void FallCheck()
            {
                List<FallInfo> fallInfos = new List<FallInfo>();
                List<Cell> visitCell = new List<Cell>();
                for(int y = 0; y < _lines.Count; ++y)
                {
                    for(int x = 0; x < ConstantData.MAX_WIDTH_NUM; ++x)
                    {
                        Cell cell = GetCell(x, y);
                        if(visitCell.IndexOf(cell) != -1)
                        {
                            continue;
                        }
                        visitCell.Add(cell);
                        if(cell == null)
                        {
                            continue;
                        }
                        if(cell.Block.HasMiddleBlock)
                        {
                            FallInfo fallInfo = new FallInfo();
                            fallInfo.ConnectedCell.Add(cell);
                            fallInfo.IsArriveEnd |= (cell.Pos.y == _lines.Count - 1);
                            GetConnectedCell(cell, ref fallInfo, ref visitCell);
                            fallInfos.Add(fallInfo);
                        }
                    }
                }

                for(int i = 0; i < fallInfos.Count; ++i)
                {
                    if (!fallInfos[i].IsArriveEnd)
                    {
                        for(int j = 0; j < fallInfos[i].ConnectedCell.Count; ++j)
                        {
                            fallInfos[i].ConnectedCell[j].Block.MiddleBlock.Hit.FallBlock();
                        }
                    }
                }

                GameController.Instance.RemoveGameState(GameController.GameState.Processing_UserInput);
                if(IsNeedGenerateBlock())
                {
                    GameController.Instance.AddGameState(GameController.GameState.GenerateEffect);
                    RequsetGenerate();
                }
            }

            private void GetConnectedCell(Cell pivotCell, ref FallInfo fallInfo, ref List<Cell> visitCell)
            {
                for(int i = 0; i < pivotCell.ArroundCell.Count; ++i)
                {
                    if (visitCell.IndexOf(pivotCell.ArroundCell[i]) != -1)
                    {
                        continue;
                    }
                    visitCell.Add(pivotCell.ArroundCell[i]);
                    if (pivotCell.ArroundCell[i] == null)
                    {
                        continue;
                    }
                    if (pivotCell.ArroundCell[i].Block.HasMiddleBlock)
                    {
                        fallInfo.ConnectedCell.Add(pivotCell.ArroundCell[i]);
                        fallInfo.IsArriveEnd |= (pivotCell.ArroundCell[i].Pos.y == _lines.Count - 1);
                        GetConnectedCell(pivotCell.ArroundCell[i], ref fallInfo, ref visitCell);
                    }
                }
            }

            #endregion

            #region Data load

            public void LoadMapData(JsonData mapRoot)
            {
                JsonData lineList = mapRoot[ConstantData.LEVEL_DATA_LINE_LIST];
                _isStartOdd = lineList[0][ConstantData.LEVEL_DATA_LINE].Count % 2 == 1;
                for(int i = 0; i < lineList.Count; ++i)
                {
                    JsonData lineRoot = lineList[i][ConstantData.LEVEL_DATA_LINE];
                    Line line = ObjectPoolController.Instance.GetLine(_trMap);
                    line.transform.localPosition = new Vector2(lineRoot.Count % 2 == 1? 0.5f:0f, ConstantData.HEIGHT_INTERVAL * i);
                    line.LoadLineData(lineRoot, i);
                    _lines.Add(line);
                }
                SetArrounCell();
            }

            private void SetArrounCell()
            {
                for(int y = 0; y < _lines.Count; ++y)
                {
                    bool isOddLine = _lines[y].IsOddLine;
                    for(int x = 0; x < ConstantData.MAX_WIDTH_NUM; ++x)
                    {
                        Vector2Int pos = new Vector2Int(x, y);
                        Cell cell = GetCell(pos);
                        if(cell.Type == CellType.GenerateCell)
                        {
                            _generateCell.Add(cell);
                        }
                        if(cell.Type == CellType.FinishCell)
                        {
                            _finishCell.Add(cell);
                        }

                        List<Cell> arroundCell = new List<Cell>();
                        Vector2Int[] directions;
                        if(isOddLine)
                        {
                            directions = CellIndex.OddLine.Directions;
                        }
                        else
                        {
                            directions = CellIndex.EvenLine.Directions;
                        }
                        for(int i = 0; i < directions.Length; ++i)
                        {
                            Vector2Int targetPos = pos + directions[i];
                            if(!CellIndex.Verification(targetPos))
                            {
                                arroundCell.Add(null);
                                continue;
                            }
                            arroundCell.Add(GetCell(targetPos));
                        }
                        cell.SetArroundCell(arroundCell);
                    }
                }
            }

            #endregion

        }
    }
}
