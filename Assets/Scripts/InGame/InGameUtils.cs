using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public static class InGameUtils
        {
            public static Vector2 GetMapPosition()
            {
                return new Vector2(0, ConstantData.DEFAULT_CAMERA_ORTHOGRAPHIC_SIZE - (ConstantData.HEIGHT_INTERVAL * ConstantData.MAX_VISIBLE_HEIGHT_NUM));
            }
        }
    }
}
