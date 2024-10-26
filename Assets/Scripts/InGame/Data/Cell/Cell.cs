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
        }

        public class Cell : BBData
        {

            #region Cell

            [Header("Cell info")]
            [SerializeField]
            private CellType _type;

            [SerializeField]
            private CellDirectionType _direction;

            [SerializeField]
            private bool _isOddLine = false;

            [SerializeField]
            private List<Cell> _arroundCell = new List<Cell>();

            #endregion

            #region Cell component

            [Header("Cell component")]
            [SerializeField]
            private CellBlock _block;
            public CellBlock Block => _block;

            #endregion

            #region Editor

#if UNITY_EDITOR

            public void Reset()
            {
                if(TryGetComponent<CellBlock>(out var block))
                {
                    _block = block;
                }
            }

#endif

            #endregion

        }
    }
}
