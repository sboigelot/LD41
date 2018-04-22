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

            return GameManager.Instance.Game.Player.Elements;
        }

        protected override void Prepare(GameObject itemObject, Element data)
        {
            itemObject.GetComponentInChildren<Text>().text = "" + data.Count;
            var elementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(data.Uri);
            SpriteManager.Set(itemObject.GetComponentInChildren<Image>(), "Images", elementPrototype.SpritePath);
            itemObject.AddComponent<Draggable>();
        }
    }
}
