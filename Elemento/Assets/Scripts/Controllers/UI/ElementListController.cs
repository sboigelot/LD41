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
    public class ElementListController : UiItemList<Element>
    {
        protected override List<Element> GetData()
        {
            if (GameManager.Instance.Game == null ||
                GameManager.Instance.Game.Player == null ||
                GameManager.Instance.Game.Player.Elements == null)
            {
                return new List<Element>();
            }

            return GameManager.Instance.Game.Player.Elements.OrderBy(e => e.Uri).ToList();
        }

        protected override void Prepare(GameObject itemObject, Element data)
        {
            var elementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(data.Uri);
            StartCoroutine(SpriteManager.Set(itemObject.GetComponentInChildren<Image>(), "Images/Elements", elementPrototype.SpritePath));
            var itemController = itemObject.AddComponent<ElementListItemController>();
            itemController.Element = data;
            itemController.ElementPrototype = elementPrototype;
            itemController.Text = itemObject.GetComponentInChildren<Text>();
            itemController.TooltipProvider = itemObject.GetComponentInChildren<TooltipProvider>();
        }
    }
}
