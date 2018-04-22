using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.Game.UI
{
    public class ElementListItemController : MonoBehaviour
    {
        public Text Text;

        public Element Element;

        public void Update()
        {
            Text.text = "" + Element.Count;
        }
    }
}