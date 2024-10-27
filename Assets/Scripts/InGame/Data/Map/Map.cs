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
