using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class BlockCache : BlockComponent
        {

            #region Using for hit

            private List<Vector2Int> _hitPositions = null;
            public List<Vector2Int> HitPositions
            {
                get { return _hitPositions; }
                set { _hitPositions = value; }
            }

            #endregion

            #region General

            public override void Init()
            {
                base.Init();
                _hitPositions = null;
            }

            public override void Dispose()
            {
                base.Dispose();
                _hitPositions = null;
            }

            #endregion

        }
    }
}
