using JH.BBS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace JH
{
    public class AddressableController : SingletonController<AddressableController>
    {

        #region Instance

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            if (_instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        #endregion

        #region Block attribute

        private Dictionary<int, BlockAttribute> _dicBlockAttribute = new Dictionary<int, BlockAttribute>();
        public BlockAttribute GetBlockAttribute(BlockType type)
        {
            int typeId = (int)type;
            if(_dicBlockAttribute.ContainsKey(typeId))
            {
                return _dicBlockAttribute[typeId];
            }

            var op = Addressables.LoadAssetAsync<BlockAttribute>(type.ToString());
            BlockAttribute blockAttribute = op.WaitForCompletion();

            _dicBlockAttribute.Add(typeId, blockAttribute);

            return blockAttribute;
        }

        #endregion

    }
}
