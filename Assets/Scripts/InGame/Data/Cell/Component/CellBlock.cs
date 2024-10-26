using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class CellBlock : CellComponent
        {

            #region Block

            [SerializeField]
            private Block _bottomBlock;
            public Block BottomBlock => _bottomBlock;
            public bool HasBottomBlock => BottomBlock != null;

            [SerializeField]
            private Block _middleBlock;
            public Block MiddleBlock => _middleBlock;
            public bool HasMiddleBlock => MiddleBlock != null;

            [SerializeField]
            private Block _topBlock;
            public Block TopBlock => _topBlock;
            public bool HasTopBlock => TopBlock != null;

            #endregion

        }
    }
}
