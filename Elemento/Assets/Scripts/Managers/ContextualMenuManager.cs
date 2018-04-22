using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Controllers;
using Assets.Scripts.Models;
using Assets.Scripts.UI.Controls.ContextualMenu;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ContextualMenuManager: MonoBehaviourSingleton<ContextualMenuManager>, IContextualMenuItemInfoProvider
    {
        public Sprite DefaultContextualMenuItemSprite;

        private bool RaycastAll(Ray ray, out RaycastHit hit)
        {
            // LayerMask layermask = new LayerMask {value = terrainGameObject.layer};

            hit = new RaycastHit();
            if (!Physics.Raycast(ray, out hit, 1000/*, layermask*/))
            {
                Debug.LogWarning("Raycast failed");
                return false;
            }
            return true;
        }

        public virtual IEnumerable<ContextualMenuItemInfo> GetContextualMenuInfo()
        {
            var interactable = GetInteractableUnderMouse();

            if (interactable == null)
            {
                yield break;
            }

            var plotController = interactable.GetComponentInChildren<TowerPlotController>();

            if (plotController != null)
            {
                var plot = plotController.Plot;
                if (plot == null)
                    yield break;

                if (plot.Tower != null)
                {
                    yield return new ContextualMenuItemInfo
                    {
                        Image = DefaultContextualMenuItemSprite,
                        IsEnable = () => plot.Editable,
                        Name = "Destroy",
                        TooltipText = "Destroy Tower",
                        OnClick = (contextualMenu, gameObject, vector3) => { plotController.DestroyTower(); }
                    };
                }
                else
                {
                    foreach (var element in GameManager.Instance.Game.Player.Elements)
                    {
                        var prototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(element.Uri);
                        yield return new ContextualMenuItemInfo
                        {
                            Image = SpriteManager.Instance.GetChached("Images/Elements", prototype.SpritePath),
                            IsEnable = () => element.Count > 0,
                            Name = "AddElement"+prototype.Name,
                            TooltipText = "Add Element " + prototype.Name,
                            OnClick = (contextualMenu, gameObject, vector3) => { plotController.AddElement(element); }
                        };

                    }
                }
            }

            yield break;
        }

        private GameObject GetInteractableUnderMouse()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (RaycastAll(ray, out hit))
            {
                return hit.collider.gameObject;
            }
            return null;
        }

        private void LogClick(Scripts.ContextualMenu menu, GameObject instanciator, Vector3 position)
        {
            Debug.Log("click on ctx menu item");
        }
    }
}
