﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    public static class ConstantData
    {

        #region Scene

        public static string LOGO_SCENE = "LogoScene";
        public static string TITLE_SCENE = "TitleScene";
        public static string PLAY_SCENE = "PlayScene";

        #endregion


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
                    heightInterval = Mathf.RoundToInt(Mathf.Sqrt(3 * CIRCLE_RADIUS * CIRCLE_RADIUS) * 1000) * 0.001f;
                }
                return heightInterval;
            }
        }

        public static float CIRCLE_RADIUS = 0.5f;

        public static int MAX_WIDTH_NUM = 10;

        public static int MAX_VISIBLE_HEIGHT_NUM = 11;

        public static float DEFAULT_CAMERA_ORTHOGRAPHIC_SIZE = 9.95f;

        public static float Degree_LIMIT = 30f;

        public static float HINT_LINE_LIMIT = 10f;

        public static int NEROS_BEEDS_NUM = 28;

        public static int BIG_BOMB_DESTROY_SCORE = 250;
        public static int WALL_SCORE = 25;

        public static int RETAIN_MOVE_SCORE = 1000;

        #endregion

        #region Level data

        public static string LEVEL_DATA_LEVEL_TYPE = "lt";
        public static string LEVEL_DATA_MOVE = "move";
        public static string LEVEL_DATA_START_ODD = "odd";
        public static string LEVEL_DATA_BOSS_HEALTH = "bh";
        public static string LEVEL_DATA_MISSION = "ms";
        public static string LEVEL_DATA_STAR_SCORES = "sc";
        
        public static string LEVEL_DATA_MAP = "map";
        
        public static string LEVEL_DATA_LINE_LIST = "ll";
        public static string LEVEL_DATA_LINE = "line";
        
        public static string LEVEL_DATA_CELL_TYPE = "ct";
        public static string LEVEL_DATA_CELL_DIRECTION = "cd";

        public static string LEVEL_DATA_GENERATE_INFO_LIST = "gil";
        public static string LEVEL_DATA_GENERATE_NUM = "gn";
        public static string LEVEL_DATA_GENERATE_WEIGHT = "gw";
        public static string LEVEL_DATA_GENERATE_REDUCE_NUM = "grn";

        public static string LEVEL_DATA_BLOCK_TYPE_LIST = "btl";
        public static string LEVEL_DATA_BLOCK_TYPE = "bt";

        #endregion

    }
}
