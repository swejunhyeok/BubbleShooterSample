using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class BlockSprite : BlockComponent
        {
            #region Sprtie

            [Header("Sprtie")]
            [SerializeField]
            private SpriteRenderer _srMain;

            [SerializeField]
            private SpriteRenderer _srFairy;

            public void SetMainSpriteSorting()
            {
                if(!Parent.HasAttribute)
                {
                    return;
                }
                if(Parent.Attribute.IsTopLayer)
                {
                    _srMain.sortingLayerID = SortingLayer.NameToID("Top");
                    return;
                }
                if(Parent.Attribute.IsMiddleLayer)
                {
                    _srMain.sortingLayerID = SortingLayer.NameToID("Middle");
                    return;
                }
                if(Parent.Attribute.IsBottomLayer)
                {
                    _srMain.sortingLayerID = SortingLayer.NameToID("Bottom");
                    return;
                }
            }

            public void SetMainSprite(Sprite sprite)
            {
                _srMain.sprite = sprite;
            }

            public void SetFairyEnable(bool isEnable)
            {
                _srFairy.enabled = isEnable;
            }

            #endregion
        }
    }
}
