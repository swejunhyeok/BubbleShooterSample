using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    public static class ConstantData
    {

        #region In Game

        private static float heightDepth = -1f;
        /// <summary>
        /// Circle object radius = 0.5f
        /// <br/>
        /// Circle object height depth = √(3 * (Circle object radius ^ 2))
        /// </summary>
        public static float HEIGHT_DEPTH
        {
            get
            {
                if(heightDepth == -1)
                {
                    heightDepth = Mathf.Sqrt(3 * heightDepth * heightDepth);
                }
                return heightDepth;
            }
        }

        public static float CIRCLE_RADIUS = 0.5f;

        public static int MAX_WIDTH_NUM = 10;

        #endregion

    }
}
