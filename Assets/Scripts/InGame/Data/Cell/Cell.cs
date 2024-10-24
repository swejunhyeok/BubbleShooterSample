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

            [SerializeField]
            private CellType _type;

            [SerializeField]
            private CellDirectionType _direction;

            [SerializeField]
            private bool _isOddLine = false;

            [SerializeField]
            private List<Cell> _arroundCell = new List<Cell>();

            #endregion


        }
    }
}
