using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class Block : BBData
        {

            #region Block

            [SerializeField]
            private BlockAttribute _attribute;
            public BlockAttribute Attribute
            {
                get
                {
                    return _attribute;
                }
            }

            #endregion

        }
    }
}
