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

            RedFairyCircle = 111,
            YellowFairyCircle,
            BlueFairyCircle,
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
            None = 0x000,
            ColorMatch = 0x001,
            ArroundMatch = 0x002,
            SpecialBlock = 0x004,
            GetShot = 0x008,
            Arrive = 0x010,
        }

        [System.Flags]
        public enum HitEffectType
        {
            None = 0x000,
            Destroy = 0x001,
            ChangeBlock = 0x002,
            Bomb = 0x004,
            LargeBomb = 0x008,
            Fairy = 0x010,
            ReduceMission = 0x020,
        }

        [CreateAssetMenu(fileName = "Block Attribute", menuName = "JH/Bubble Shooter Sample/Block Attribute")]
        public class BlockAttribute : ScriptableObject
        {

            #region Common

            [Header("Common")]
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
            public bool IsBottomLayer => (_layer & LayerType.Bottom) == LayerType.Bottom;
            public bool IsMiddleLayer => (_layer & LayerType.Middle) == LayerType.Middle;
            public bool IsTopLayer => (_layer & LayerType.Top) == LayerType.Top;

            [SerializeField]
            private ColorType _color;
            public ColorType Color
            {
                get { return _color; }
            }

            [SerializeField]
            private Sprite _sprBlock;
            public Sprite SprBlock
            {
                get { return _sprBlock; }
            }

            #endregion

            #region Hit

            [Header("Hit")]
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

            [SerializeField]
            private BlockAttribute _hitChangeBlock;
            public BlockAttribute HitChangeBlock
            {
                get { return _hitChangeBlock; }
            }

            #endregion

            #region Score

            [Header("Score")]
            [SerializeField]
            private int _hitScore;
            public int HitScore
            {
                get { return _hitScore; }
            }

            [SerializeField]
            private bool _isHitScoreMultiplyComb;
            public bool IsHitScoreMultiplyComb
            {
                get { return _isHitScoreMultiplyComb; }
            }

            [SerializeField]
            private int _fallScore;
            public int FallScore
            {
                get { return _fallScore; }
            }

            [SerializeField]
            private bool _isFallScoreMultiplyComb;
            public bool IsFallScoreMultiplyComb
            {
                get { return _isFallScoreMultiplyComb; }
            }

            [SerializeField]
            private int _retainScore;
            public int RetainScore
            {
                get { return _retainScore; }
            }

            #endregion

        }
    }
}
