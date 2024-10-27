using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class BlockComponent : MonoBehaviour
        {
            [Header("Component")]
            [SerializeField]
            private Block _parent;
            public Block Parent => _parent;

            #region Editor

#if UNITY_EDITOR

            public virtual void Reset()
            {
                if(TryGetComponent<Block>(out var block))
                {
                    _parent = block;
                }
            }

#endif

            #endregion
        }
    }
}
