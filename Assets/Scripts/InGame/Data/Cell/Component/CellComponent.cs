using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class CellComponent : MonoBehaviour
        {

            #region Cell

            [Header("Cell component")]
            [SerializeField]
            protected Cell _parent;
            public Cell Parent
            {
                get { return _parent; }
            }

            #endregion

            #region Editor

#if UNITY_EDITOR

            protected void RegistParent()
            {
                if (TryGetComponent<Cell>(out var parent))
                {
                    _parent = parent;
                }
            }

            protected virtual void Reset()
            {
                RegistParent();
            }

#endif

            #endregion
        }
    }
}
