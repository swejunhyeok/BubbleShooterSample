using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public static class InGameUtils
        {

            public static bool CellVerification(Cell cell)
            {
                if(cell == null)
                {
                    return false;
                }
                if(cell.Type == CellType.None)
                {
                    return false;
                }
                return true;
            }

            public static float GetTimeDepth(float time)
            {
                if (time == 0)
                {
                    return 0;
                }
                return 1.0f / time;
            }

            public static int ParseInt(ref LitJson.JsonData root, string key, int defaultValue = 0) => root.Keys.Contains(key) ? (int)root[key] : defaultValue;
            public static Vector2 GetMapPosition()
            {
                return new Vector2(0, ConstantData.DEFAULT_CAMERA_ORTHOGRAPHIC_SIZE - (ConstantData.HEIGHT_INTERVAL * (ConstantData.MAX_VISIBLE_HEIGHT_NUM - 1)));
            }
        }
    }
}
