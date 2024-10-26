using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class UIController : SingletonController<UIController>
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
            }

            #endregion

            #region Unity component

            [Header("Unity component")]
            [SerializeField]
            private Camera _cameraMain;

            [SerializeField]
            private RectTransform _trMainSponer;

            private Vector2 _posMainSponer = Vector2.one * int.MinValue;
            public Vector2 PosMainSponer
            {
                get
                {
                    if (_posMainSponer == Vector2.one * int.MinValue)
                    {
                        _posMainSponer = _cameraMain.ScreenToWorldPoint(_trMainSponer.transform.position);
                    }
                    return _posMainSponer;
                }
            }

            #endregion

            #region Editor

#if UNITY_EDITOR

            [EditorButton("Debug sponer position", "DebugSponerPosition")]
            public void DebugSponerPosition()
            {
                Debug.Log(PosMainSponer.ToString());
            }

#endif

            #endregion

        }
    }
}
