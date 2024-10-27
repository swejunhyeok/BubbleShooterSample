using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class Line : BBData
        {

            #region Line

            [Header("Line")]
            [SerializeField]
            private Transform _trLine;

            [SerializeField]
            private bool _isOddLine = false;
            public bool IsOddLine => _isOddLine;

            [SerializeField]
            private int _index;

            #endregion

            #region Cell

            [Header("Cell")]
            [SerializeField]
            private Cell[] _cells = new Cell[ConstantData.MAX_WIDTH_NUM];

            public Cell GetCell(int index)
            {
                if(index < 0 || index >= _cells.Length)
                {
                    return null;
                }
                return _cells[index];
            }

            public bool IsEmptyLine
            {
                get
                {
                    for (int i = 0; i < _cells.Length; ++i)
                    {
                        if (_cells[i] == null)
                        {
                            continue;
                        }
                        if (_cells[i].Type == CellType.None)
                        {
                            continue;
                        }
                        if (_cells[i].Type == CellType.GenerateCell)
                        {
                            return false;
                        }
                        if (_cells[i].Block.IsEmpty)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }

            #endregion

            #region Data load

            public void LoadLineData(JsonData lineRoot, int index)
            {
                _index = index;
                _isOddLine = lineRoot.Count % 2 == 1;
                if(_cells == null || _cells.Length != ConstantData.MAX_WIDTH_NUM)
                {
                    _cells = new Cell[ConstantData.MAX_WIDTH_NUM];
                }

                for (int i = 0; i < lineRoot.Count; ++i)
                {
                    if (_cells[i] == null)
                    {
                        _cells[i] = ObjectPoolController.Instance.GetCell(_trLine);
                    }
                    _cells[i].transform.localPosition = new Vector2(-ConstantData.CIRCLE_RADIUS * (ConstantData.MAX_WIDTH_NUM - 1), 0);
                    _cells[i].LoadCellData(lineRoot[i], _isOddLine, _index, i);
                }
            }

            #endregion

        }
    }
}
