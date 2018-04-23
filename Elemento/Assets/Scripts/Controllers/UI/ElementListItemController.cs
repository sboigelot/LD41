using Assets.Scripts.Models;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.Game.UI
{
    public class ElementListItemController : MonoBehaviour
    {
        public Text Text;

        public Element Element;
        public ElementPrototype ElementPrototype;

        public TooltipProvider TooltipProvider;

        public void Update()
        {
            Text.text = "" + Element.Count;
            TooltipProvider.content = ElementPrototype.Name;
        }
    }
}