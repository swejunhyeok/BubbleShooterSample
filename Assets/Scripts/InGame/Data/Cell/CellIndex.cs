using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public enum CellDirectionType
        {
            None = -1,
            RightUp,
            Right,
            RightDown,
            LeftDown,
            Left,
            LeftUp,
            Cnt,
        }

        public static class CellIndex
        {
            public static Vector2Int None = new Vector2Int(int.MinValue, int.MinValue);

            public static class OddLine
            {
                public static Vector2Int RightUpDirection = new Vector2Int(1, 1);
                public static Vector2Int RightDirection = new Vector2Int(1, 0);
                public static Vector2Int RightDownDirection = new Vector2Int(1, -1);
                public static Vector2Int LeftDownDirection = new Vector2Int(0, -1);
                public static Vector2Int LeftDirection = new Vector2Int(-1, 0);
                public static Vector2Int LeftUpDirection = new Vector2Int(0, 1);
                public static Vector2Int[] Directions = { RightUpDirection, RightDirection, RightDownDirection, LeftDownDirection, LeftDirection, LeftUpDirection };
            }

            public static class EvenLine
            {
                public static Vector2Int RightUpDirection = new Vector2Int(0, 1);
                public static Vector2Int RightDirection = new Vector2Int(1, 0);
                public static Vector2Int RightDownDirection = new Vector2Int(0, -1);
                public static Vector2Int LeftDownDirection= new Vector2Int(-1, -1);
                public static Vector2Int LeftDirection = new Vector2Int(-1, 0);
                public static Vector2Int LeftUpDirection = new Vector2Int(-1, 1);
                public static Vector2Int[] Directions = { RightUpDirection, RightDirection, RightDownDirection, LeftDownDirection, LeftDirection, LeftUpDirection };
            }

            public static bool Verification(Vector2Int pos)
            {
                if(pos.x < 0 || pos.y < 0)
                {
                    return false;
                }
                if(pos.x >= ConstantData.MAX_WIDTH_NUM)
                {
                    return false;
                }
                return true;
            }

            public static bool Verification(Vector2Int pos, Vector2Int dir)
            {
                return Verification(pos + dir);
            }
        }
    }
}
