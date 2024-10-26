using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    public static class ConstantData
    {

        #region In Game

        private static float heightInterval = -1f;
        /// <summary>
        /// Circle object radius = 0.5f
        /// <br/>
        /// Circle object height interval = √(3 * (Circle object radius ^ 2))
        /// </summary>
        public static float HEIGHT_INTERVAL
        {
            get
            {
                if(heightInterval == -1)
                {
                    heightInterval = Mathf.Round(Mathf.Sqrt(3 * heightInterval * heightInterval) * 1000) * 0.001f;
                }
                return heightInterval;
            }
        }

        public static float CIRCLE_RADIUS = 0.5f;

        public static int MAX_WIDTH_NUM = 10;

        public static int MAX_VISIBLE_HEIGHT_NUM = 11;

        public static float DEFAULT_CAMERA_ORTHOGRAPHIC_SIZE = 9.95f;

        public static float Degree_LIMIT = 30f;

        #endregion

    }
}
