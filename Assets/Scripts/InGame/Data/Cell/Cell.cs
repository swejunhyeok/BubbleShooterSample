using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public enum CellType
        {
            None,
            NormalCell,
            GenerateCell,
            FinishCell,
        }

        public class Cell : BBData
        {

            #region Cell

            [Header("Cell info")]
            [SerializeField]
            private Vector2Int _pos;
            public Vector2Int Pos => _pos;

            [SerializeField]
            private int _index;
            public int Index => _index;

            [SerializeField]
            private CellType _type;
            public CellType Type => _type;

            [SerializeField]
            private CellDirectionType _direction = CellDirectionType.None;

            [SerializeField]
            private bool _isOddLine = false;

            [SerializeField]
            private List<Cell> _arroundCell = new List<Cell>();
            public Cell RightUpCell => _arroundCell[(int)CellDirectionType.RightUp];
            public Cell RightCell => _arroundCell[(int)CellDirectionType.Right];
            public Cell RightDownCell => _arroundCell[(int)CellDirectionType.RightDown];
            public Cell LeftDownCell => _arroundCell[(int)CellDirectionType.LeftDown];
            public Cell LeftCell => _arroundCell[(int)CellDirectionType.Left];
            public Cell LeftUpCell => _arroundCell[(int)CellDirectionType.LeftUp];

            public void SetArroundCell(List<Cell> arroundCell) => _arroundCell = new List<Cell>(arroundCell);

            #endregion

            #region Cell component

            [Header("Cell component")]
            [SerializeField]
            private CellBlock _block;
            public CellBlock Block => _block;

            [SerializeField]
            private CellGenerate _generate;
            public CellGenerate Generate => _generate;

            #endregion

            #region Data load

            public void LoadCellData(JsonData cellRoot, bool isOddLine, int lineIndex, int cellIndex)
            {
                _pos = new Vector2Int(cellIndex, lineIndex);
                _index = lineIndex * ConstantData.MAX_WIDTH_NUM + cellIndex;
                _isOddLine = isOddLine;
                _type = (CellType)InGameUtils.ParseInt(ref cellRoot, ConstantData.LEVEL_DATA_CELL_TYPE, 0);
                _direction = (CellDirectionType)InGameUtils.ParseInt(ref cellRoot, ConstantData.LEVEL_DATA_CELL_DIRECTION, -1);
                if(_type == CellType.GenerateCell)
                {
                    if(cellRoot.ContainsKey(ConstantData.LEVEL_DATA_GENERATE_INFO_LIST))
                    {
                        Generate.LoadGenerateData(cellRoot[ConstantData.LEVEL_DATA_GENERATE_INFO_LIST]);
                    }
                }
            }

            #endregion

            #region Editor

#if UNITY_EDITOR

            public void Reset()
            {
                if(TryGetComponent<CellBlock>(out var block))
                {
                    _block = block;
                }
                if(TryGetComponent<CellGenerate>(out var generate))
                {
                    _generate = generate;
                }
            }

#endif

            #endregion

        }
    }
}
