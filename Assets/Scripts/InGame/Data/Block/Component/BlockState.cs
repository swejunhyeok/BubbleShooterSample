using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {

        public enum BlockStateType
        {
            Idle,
            Move,
            Destroy,
            Dispose,
        }

        public class BlockState : BlockComponent
        {

            #region State

            [Header("State")]
            [SerializeField]
            private BlockStateType _state;
            public BlockStateType State => _state;

            public void SetState(BlockStateType state)
            {
                _state = state;
            }

            #endregion

            #region General

            public override void Init()
            {
                base.Init();
                _state = BlockStateType.Idle;
            }

            public override void Dispose()
            {
                base.Dispose();
                _state = BlockStateType.Dispose;
            }

            #endregion

        }
    }
}
