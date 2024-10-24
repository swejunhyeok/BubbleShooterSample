using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {

        [System.Flags]
        public enum LayerType
        {
            None = 0x00001,
            Bottom = 0x00010,
            Middle = 0x00100,
            Top = 0x01000,
        }

        [System.Flags]
        public enum BlockType
        {
            None = 0,

            ___DefaultBlock___ = 1,
            RedCircle,
            YellowCircle,
            BlueCircle,

            ___SpecialBlock___ = 101,
            BombCircle,
            BigBombCircle,

            RedFlyCircle = 101,
            YellowFlyCircle,
            BlueFlyCircle,
        }

        [System.Flags]
        public enum ColorType
        {
            None = 0x000,
            Red = 0x001,
            Yellow = 0x002,
            Blue = 0x004,
        }

        [System.Flags]
        public enum HitConditionType
        {
            None,
            ColorMatch,
            ArroundMatch,
            SpecialBlock,
            GetShot,
        }

        [System.Flags]
        public enum HitEffectType
        {
            None,
            Destroy,
            ChangeBlock,
            Bomb,
            LargeBomb,
        }

        [CreateAssetMenu(fileName = "Block Attribute", menuName = "JH/Bubble Shooter Sample/Block Attribute")]
        public class BlockAttribute : ScriptableObject
        {

            #region Common

            [SerializeField]
            private BlockType _type;
            public BlockType Type
            {
                get { return _type; }
            }

            [SerializeField]
            private int _health;
            public int Health
            {
                get { return _health; }
            }

            [SerializeField]
            private LayerType _layer;
            public LayerType Layer
            {
                get { return _layer; }
            }

            [SerializeField]
            private ColorType _color;
            public ColorType Color
            {
                get { return _color; }
            }

            #endregion

            #region Hit

            [SerializeField]
            private HitConditionType _hitCondition;
            public HitConditionType HitCondition
            {
                get { return _hitCondition; }
            }

            [SerializeField]
            private HitEffectType _hitEffect;
            public HitEffectType HitEffect
            {
                get { return _hitEffect; }
            }

            #endregion

        }
    }
}
