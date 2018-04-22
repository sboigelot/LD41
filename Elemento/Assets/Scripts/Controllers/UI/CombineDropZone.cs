using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Controllers.Framework;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.Game.UI
{
    public class CombineDropZone : DropZone
    {
        public Image Image;

        public Element Element;

        public CombineButton CombineButton;

        public Sprite NoElementSprite;

        public override void OnDrop(Draggable draggable)
        {
            var listItem = draggable.gameObject.GetComponent<UiItemListItem>();
            if (listItem == null || listItem.Data == null)
            {
                return;
            }

            var element = listItem.Data as Element;
            if (element == null)
            {
                return;
            }

            Element = element;
            Redraw();
        }

        public void Redraw()
        {
            if (Element.Count <= 0)
            {
                Image.sprite = NoElementSprite;
                Element = null;
                return;
            }

            var elementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(Element.Uri);
            StartCoroutine(SpriteManager.Set(Image, "Images/Elements", elementPrototype.SpritePath));

            CombineButton.EvaluateReciepe();
        }
    }
}
