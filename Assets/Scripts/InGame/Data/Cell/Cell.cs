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
            public CellDirectionType Direction => _direction;

            [SerializeField]
            private bool _isOddLine = false;

            [SerializeField]
            private List<Cell> _arroundCell = new List<Cell>();
            public List<Cell> ArroundCell => _arroundCell;
            public Cell RightUpCell => _arroundCell[(int)CellDirectionType.RightUp];
            public Cell RightCell => _arroundCell[(int)CellDirectionType.Right];
            public Cell RightDownCell => _arroundCell[(int)CellDirectionType.RightDown];
            public Cell LeftDownCell => _arroundCell[(int)CellDirectionType.LeftDown];
            public Cell LeftCell => _arroundCell[(int)CellDirectionType.Left];
            public Cell LeftUpCell => _arroundCell[(int)CellDirectionType.LeftUp];

            public void SetArroundCell(List<Cell> arroundCell) => _arroundCell = new List<Cell>(arroundCell);
            public Cell GetArroundCell(int index)
            {
                if(index < 0 || index >= _arroundCell.Count)
                {
                    return null;
                }
                return _arroundCell[index];
            }

            [SerializeField]
            private SpriteRenderer _srHint;

            public void SetHintEnable(bool enable)
            {
                _srHint.enabled = enable;
            }

            #endregion

            #region Cell component

            [Header("Cell component")]
            [SerializeField]
            private CellBlock _block;
            public CellBlock Block => _block;

            [SerializeField]
            private CellGenerate _generate;
            public CellGenerate Generate => _generate;

            [SerializeField]
            private CellEffect _effect;
            public CellEffect Effect => _effect;

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

            #region General

            public override void Init()
            {
                base.Init();
                
                _block.Init();
                _generate.Init();
                _effect.Init();
            }

            public override void Dispose()
            {
                base.Dispose();

                _block.Dispose();
                _generate.Dispose();
                _effect.Dispose();
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
                if(TryGetComponent<CellEffect>(out var effect))
                {
                    _effect = effect;
                }
            }

#endif

            #endregion

        }
    }
}
