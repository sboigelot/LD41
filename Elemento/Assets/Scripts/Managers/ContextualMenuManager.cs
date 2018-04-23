using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Controllers;
using Assets.Scripts.Controllers.Framework.ContextualMenu;
using Assets.Scripts.Models;
using Assets.Scripts.UI.Controls.ContextualMenu;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ContextualMenuManager: MonoBehaviourSingleton<ContextualMenuManager>, IContextualMenuItemInfoProvider
    {
        public Sprite DefaultContextualMenuItemSprite;

        public ContextualMenuHost ContextualMenuHost;

        private GameObject lastInteractable;

        public Sprite DestroySprite;

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

        public virtual IEnumerable<ContextualMenuItemInfo> GetContextualMenuInfo(bool reOpen)
        {
            var interactable = reOpen ? lastInteractable : GetInteractableUnderMouse();

            if (interactable == null)
            {
                yield break;
            }

            lastInteractable = interactable;

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
                        Image = DestroySprite,
                        IsEnable = () => plot.Editable,
                        Name = "Destroy",
                        TooltipText = "Destroy Tower",
                        OnClick = (contextualMenu, gameObject, vector3) => { plotController.DestroyTower(); }
                    };
                }
                else
                {
                    var buildingStage = plotController.BuildingElements == null ||
                                        plotController.BuildingElements.Count == 0 ? TowerSlotType.Base :
                                        plotController.BuildingElements.Count == 1 ? TowerSlotType.Body :  
                                        TowerSlotType.Weapon;
                    var partNames = new Dictionary<TowerSlotType, string>
                    {
                        {TowerSlotType.Base, "pedestal"},
                        {TowerSlotType.Body, "body"},
                        {TowerSlotType.Weapon, "weapon"},
                    };
                    foreach (var element in GameManager.Instance.Game.Player.Elements.Where(e=>e.Count>0))
                    {
                        var prototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(element.Uri);

                        if (prototype.ElementStats == null ||
                            prototype.ElementStats.All(s => s.InSlot != buildingStage))
                        {
                            continue;
                        }

                        var count = element.Count;
                        var playerHasElement = count > 0;

                        var element1 = element;
                        yield return new ContextualMenuItemInfo
                        {
                            Image = SpriteManager.Instance.GetChached("Images/Elements", prototype.SpritePath),
                            IsEnable = () => playerHasElement,
                            Name = "AddElement" + prototype.Name,
                            TooltipText = "Infuse " + prototype.Name + " as "+ partNames[buildingStage],
                            OnClick = (contextualMenu, go, vector3) =>
                            {
                                if (!GameManager.Instance.Game.Player.HasElement(element1))
                                {
                                    return;
                                }
                                var cost = new Element
                                {
                                    Count = 1,
                                    Uri = element1.Uri
                                };
                                plotController.AddElement(cost);
                                GameManager.Instance.Game.Player.RemoveElement(cost);
                                if (plotController.Plot.Tower == null)
                                {
                                    ContextualMenuHost.ReOpen();
                                }
                            }
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
    }
}
