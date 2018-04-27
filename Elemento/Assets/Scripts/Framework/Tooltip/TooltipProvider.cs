using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class TooltipProvider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Content;
        
        public Dictionary<string, string> MultipleContent;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (MultipleContent != null)
            {
                foreach (var item in MultipleContent)
                {
                    TooltipController.Instances[item.Key].Show(item.Value);
                }
            }
            else
            {
                TooltipController.Instance.Show(Content);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (MultipleContent != null)
            {
                foreach (var item in MultipleContent)
                {
                    TooltipController.Instances[item.Key].Hide();
                }
            }
            else
            {
                TooltipController.Instance.Hide();
            }
        }
    }
}