using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class WorldTooltipProvider : MonoBehaviour
    {
        public string content;
        public Dictionary<string, string> MultipleContent;
        private bool underMouse;

        public void FixedUpdate()
        {
            if (IsThisUnderMouse())
            {
                underMouse = true;
                if (MultipleContent != null)
                {
                    foreach (var item in MultipleContent)
                    {
                        TooltipController.Instances[item.Key].Show(item.Value);
                    }
                }
                else
                {
                    TooltipController.Instance.Show(content);
                }
            }
            else if(underMouse)
            {
                underMouse = false;
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

        private bool IsThisUnderMouse()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (RaycastAll(ray, out hit))
            {
                return hit.collider.gameObject == gameObject;
            }
            return false;
        }

        private bool RaycastAll(Ray ray, out RaycastHit hit)
        {
            // LayerMask layermask = new LayerMask {value = terrainGameObject.layer};

            hit = new RaycastHit();
            if (!Physics.Raycast(ray, out hit, 1000/*, layermask*/))
            {
                //Debug.LogWarning("Raycast failed");
                return false;
            }
            return true;
        }
    }
}