using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controllers.Game;
using Assets.Scripts.Controllers.Game.UI;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class TowerPlotController : DropZone
    {
        public TowerPlot Plot;

        private GameObject rangeSphere;

        public List<string> BuildingElements = new List<string>();

        public List<GameObject> FloatingElements = new List<GameObject>();
        public GameObject Weapon;
        public float WeaponCenter;

        public GameObject SpeedBar;
        private WorldTooltipProvider tooltip;

        public void Build(TowerPlot plot, Vector3 position, GameObject instance, TowerPlotController plotController)
        {
            plotController.Plot = plot;
            if (!plot.Tower.IsStronghold)
            {
                var baseElementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(plot.Tower.BaseElementUri);
                var bodyElementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(plot.Tower.BodyElementUri);
                var weaponElementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(plot.Tower.WeaponElementUri);

                var basePart = AddTowerPart(baseElementPrototype, TowerSlotType.Base, position, instance);

                var h = basePart.GetComponent<Renderer>().bounds.size.y;
                var bodyPart = AddTowerPart(bodyElementPrototype, TowerSlotType.Body, position + new Vector3(0, h, 0), instance);

                h += bodyPart.GetComponent<Renderer>().bounds.size.y;
                plotController.Weapon = AddTowerPart(weaponElementPrototype, TowerSlotType.Weapon, position + new Vector3(0, h, 0), instance);
                plotController.WeaponCenter = h + plotController.Weapon.GetComponent<Renderer>().bounds.size.y / 2;
                
                tooltip = instance.AddComponent<WorldTooltipProvider>();
                UpdateTooltip(baseElementPrototype, bodyElementPrototype, weaponElementPrototype);
            }
        }

        private void UpdateTooltip(ElementPrototype baseElementPrototype, ElementPrototype bodyElementPrototype, ElementPrototype weaponElementPrototype)
        {
            var title = string.Format("Tower: {0}, {1}, {2}", baseElementPrototype.Name,
                bodyElementPrototype.Name, weaponElementPrototype.Name);

            var percent = (GameManager.Instance.Game.GameTime - Plot.Tower.LastShot) / Plot.Tower.GetSpeed();
            string stats = "Range: " + Plot.Tower.GetRange() + Environment.NewLine +
                "Speed: Shoot every " + Plot.Tower.GetSpeed() + " seconds" + Environment.NewLine +
                //"Shoot% " + percent * 100 + "%" + Environment.NewLine +
                //"delta " + (GameManager.Instance.Game.GameTime - Plot.Tower.LastShot) + Environment.NewLine +
                "Damages: " + Environment.NewLine;
            var damages = Plot.Tower.GetDamage();
            foreach (var damage in damages)
            {
                if (damage.Value != 0)
                {
                    stats += "\t" + damage.Value + " " + damage.Key + Environment.NewLine;
                }
            }

            tooltip.MultipleContent = new Dictionary<string, string>
                {
                    {"default", title},
                    { "ElementsStats", stats }
                };
        }

        private GameObject AddTowerPart(ElementPrototype elementPrototype, TowerSlotType slotType, Vector3 position, GameObject instance)
        {
            var stat = elementPrototype.ElementStats.FirstOrDefault(s => s.InSlot == slotType);
            string prefabName = "tower:base";
            if (stat == null)
            {
                Debug.LogWarningFormat("AddTowerPart Failed for {0} part not found {1}", elementPrototype.Name, slotType);
            }
            else
            {
                prefabName = stat.ModelPrefab;
            }
            var prefab = PrefabManager.Instance.GetPrefab(prefabName);
            var part = Instantiate(prefab, position, Quaternion.identity, instance.transform);
            var partRenderer = part.GetComponent<MeshRenderer>();
            var albedo = partRenderer.material.GetTexture("_MainTex");
            partRenderer.material = Instantiate(PrefabManager.Instance.ProgressMaterial);
            partRenderer.material.SetColor("_Color", Color.white);
            partRenderer.material.SetTexture("_MainTex", albedo);
            partRenderer.material.SetFloat("shiftY", 30f);
            return part;
        }

        public void DestroyTower()
        {
            SoundController.Instance.PlaySound(SoundController.Instance.Build);
            Destroy(gameObject);
            Plot.Tower = null;
            MapRenderer.Instance.InstanciatePlot(Plot);
        }

        public override void OnDrop(Draggable draggable)
        {
            var item = draggable.gameObject.GetComponent<ElementListItemController>();
            if (item == null)
            {
                return;
            }

            if (item.Element.Count < 1)
            {
                SoundController.Instance.PlaySound(SoundController.Instance.Explosion);
                return;
            }

            var buildingStage = BuildingElements == null ||
                                       BuildingElements.Count == 0 ? TowerSlotType.Base :
                                       BuildingElements.Count == 1 ? TowerSlotType.Body :
                                       TowerSlotType.Weapon;

            var prototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(item.Element.Uri);
            if (prototype.ElementStats == null ||
                prototype.ElementStats.All(s => s.InSlot != buildingStage))
            {
                SoundController.Instance.PlaySound(SoundController.Instance.Explosion);
                return;
            }

            var cost = new Element
            {
                Count = 1,
                Uri = item.Element.Uri
            };

            AddElement(cost);
            GameManager.Instance.Game.Player.RemoveElement(cost);
        }

        public void AddElement(Element element)
        {
            if (Plot.Tower == null)
            {
                SoundController.Instance.PlaySound(SoundController.Instance.Scan);
                UiManager.Instance.ElementList.ReBuild();

                BuildingElements.Add(element.Uri);
                if (BuildingElements.Count >= 3)
                {
                    SoundController.Instance.PlaySound(SoundController.Instance.Build);
                    Plot.Tower = new Tower
                    {
                        BaseElementUri = BuildingElements[0],
                        BodyElementUri = BuildingElements[1],
                        WeaponElementUri = BuildingElements[2]
                    };

                    foreach (var floatingElement in FloatingElements)
                    {
                        Destroy(floatingElement);
                    }
                    Destroy(gameObject);
                    MapRenderer.Instance.InstanciateTower(Plot);
                }
                else
                {
                    AddFloatingElement(element);
                }
            }
            else
            {
                Plot.Tower.EnchantElementUri = element.Uri;
            }
        }

        private void AddFloatingElement(Element element)
        {
            var position = new Vector3(
                transform.position.x,
                transform.position.y + 1.5f * FloatingElements.Count + 1f,
                transform.position.z);
            var floating = MapRenderer.Instance.InstanciateFloatingElement(position, element, true, OnCollectFloatingElement);
            
            FloatingElements.Add(floating);
        }

        private void OnCollectFloatingElement(FloatingElementController floating)
        {
            BuildingElements.Remove(BuildingElements.FirstOrDefault(e => e == floating.Element.Uri));
            FloatingElements.Remove(floating.gameObject);
            StartCoroutine(MoveFloatingElementsDown());
            ContextualMenuManager.Instance.ContextualMenuHost.ReOpen();
        }

        private IEnumerator MoveFloatingElementsDown()
        {
            yield return new WaitForSeconds(0.4f);
            for (var index = 0; index < FloatingElements.Count; index++)
            {
                var position = new Vector3(
                    transform.position.x,
                    transform.position.y + 1.5f * index + 1f,
                    transform.position.z);
                var floatingElement = FloatingElements[index];
                floatingElement.transform.position = position;
            }
        }

        public void Update()
        {
            if (!PrototypeManager.Instance.Loaded ||
                Plot == null ||
                Plot.Tower == null ||
                Plot.Tower.IsStronghold)
            {
                return;
            }

            DisplayRangeSphere();
            
            //var baseElementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(Plot.Tower.BaseElementUri);
            //var bodyElementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(Plot.Tower.BodyElementUri);
            //var weaponElementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(Plot.Tower.WeaponElementUri);
            //UpdateTooltip(baseElementPrototype, bodyElementPrototype, weaponElementPrototype);
            UpdateSpeedBar();
        }

        private void DisplayRangeSphere()
        {
            var visible = IsThisUnderMouse();

            if (rangeSphere == null)
            {
                rangeSphere = Instantiate(PrefabManager.Instance.GetPrefab("rangesphere"), transform);
                rangeSphere.transform.localPosition = new Vector3(0, WeaponCenter, 0);
            }

            rangeSphere.SetActive(visible);

            if (visible)
            {
                var range = Plot.Tower.GetRange();
                rangeSphere.transform.localScale = new Vector3(range, range, range) * 2;
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
        
        public void FixedUpdate()
        {
            if (GameManager.Instance.Game == null ||
                GameManager.Instance.Game.Paused ||
                !PrototypeManager.Instance.Loaded ||
                Plot == null ||
                Plot.Tower == null ||
                Plot.Tower.IsStronghold)
            {
                return;
            }

            var ennemyInRange = Plot.Tower.FindTarget(transform.position);
            if (ennemyInRange == null)
            {
                return;
            }

            Quaternion newRotation =
                Quaternion.LookRotation(ennemyInRange.transform.position - transform.position,
                Vector3.forward);
            newRotation.x = 0;
            newRotation.z = 0;
            Weapon.transform.rotation = newRotation;
            
            if (!Plot.Tower.ReadyToShoot)
            {
                return;
            }

            SoundController.Instance.PlaySound(SoundController.Instance.Explosion);
            var ammoInfo = Plot.Tower.ShootAtTarget(ennemyInRange);
            ammoInfo.Origin = new Vector3(transform.position.x, WeaponCenter, transform.position.z);

            var prefab = PrefabManager.Instance.GetPrefab(ammoInfo.PrefabName);
            var ammoObject = Instantiate(prefab, ammoInfo.Origin, Quaternion.identity);
            var ammoController = ammoObject.AddComponent<AmmoController>();
            ammoController.AmmoInfo = ammoInfo;
        }
        
        private void UpdateSpeedBar()
        {
            float maxY = 0;
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                var partRenderer = child.GetComponent<MeshRenderer>();
                maxY += partRenderer.bounds.size.y;
            }

            var percent = (GameManager.Instance.Game.GameTime - Plot.Tower.LastShot) / Plot.Tower.GetSpeed();
            percent /= 4f;
            if (percent > 1f) percent = 1f;

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                var partRenderer = child.GetComponent<MeshRenderer>();
                var shiftY = transform.position.y + (maxY) * percent;
                partRenderer.material.SetFloat("shiftY", shiftY);
            }
        }
    }
}
