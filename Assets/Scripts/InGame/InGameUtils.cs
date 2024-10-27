using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {

        /// <summary>
        /// y = gradient * x + delta;
        /// </summary>
        public struct LineEquationInfo
        {
            public float Gradient;
            public float Delta;
            public bool IsPositiveDirection;
        }

        public static class InGameUtils
        {
            #region Equation

            public static float ComputeGradient(float degree) => Mathf.Tan(degree * Mathf.Deg2Rad);
            public static float ComputeDelta(float gradient, Vector2 pos) => (-gradient * pos.x) + pos.y;
            public static float ComputeLineEquation(LineEquationInfo lineEquationInfo, float x) => lineEquationInfo.Gradient * x + lineEquationInfo.Delta;
            public static float ComputeDistance(LineEquationInfo lineEquationInfo, Vector2 pos) =>
                Mathf.Abs(pos.x * lineEquationInfo.Gradient - pos.y + lineEquationInfo.Delta) /
                Mathf.Sqrt(lineEquationInfo.Gradient * lineEquationInfo.Gradient + 1);

            #endregion

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
