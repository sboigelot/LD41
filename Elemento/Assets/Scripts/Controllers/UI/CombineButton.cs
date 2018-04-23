using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.Game.UI
{
    public class CombineButton : MonoBehaviour
    {
        public CombineDropZone DropZone1;
        public CombineDropZone DropZone2;
        public Image Image;

        public Sprite NoReceipeSprite;
        
        private List<ElementPrototypeReceipe> elementPrototypeReceipes;
        private ElementPrototypeReceipe matchingReceipe;
        public void EvaluateReciepe()
        {
            bool valid =
                DropZone1.Element != null &&
                DropZone2.Element != null &&
                DropZone1.Element.Uri != DropZone2.Element.Uri &&
                EvaluateDropZone(DropZone1) &&
                EvaluateDropZone(DropZone2);
            if (!valid)
            {
                SayNo();
                return;
            }

            if (elementPrototypeReceipes == null)
            {
                elementPrototypeReceipes = PrototypeManager.Instance.GetAllPrototypes<ElementPrototypeReceipe>();
            }

            matchingReceipe = elementPrototypeReceipes.FirstOrDefault(p =>
                (p.Element1 == DropZone1.Element.Uri || p.Element2 == DropZone1.Element.Uri) &&
                (p.Element1 == DropZone2.Element.Uri || p.Element2 == DropZone2.Element.Uri));

            if (matchingReceipe == null)
            {
                SayNo();
                return;
            }

            var elementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(matchingReceipe.ElementResult);
            StartCoroutine(SpriteManager.Set(Image, "Images/Elements", elementPrototype.SpritePath));
            GetComponent<Button>().interactable = true;
        }

        private void SayNo()
        {
            Image.sprite = NoReceipeSprite;
            GetComponent<Button>().interactable = false;
        }

        private bool EvaluateDropZone(CombineDropZone dropZone)
        {
            if (dropZone.Element != null && dropZone.Element.Count == 0)
            {
                dropZone.Redraw();
                return false;
            }
            return true;
        }

        public void Combine()
        {
            if (matchingReceipe == null)
            {
                return;
            }

            var player = GameManager.Instance.Game.Player;

            var ingredient1 = new Element { Count = 1, Uri = matchingReceipe.Element1 };
            var ingredient2 = new Element { Count = 1, Uri = matchingReceipe.Element2 };

            if (player.HasElement(ingredient1) && player.HasElement(ingredient2))
            {
                SoundController.Instance.PlaySound(SoundController.Instance.Scan);
                player.RemoveElement(ingredient1);
                player.RemoveElement(ingredient2);
                player.AddElement(new Element { Count = 1, Uri = matchingReceipe.ElementResult });
            }

            UiManager.Instance.ElementList.ReBuild();
            EvaluateReciepe();
        }
    }
}
