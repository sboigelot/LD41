using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controllers.Game;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class TowerPlotController : MonoBehaviour
    {
        public TowerPlot Plot;

        private GameObject rangeSphere;

        public List<string> BuildingElements = new List<string>();

        public List<GameObject> FloatingElements = new List<GameObject>();
        public GameObject Weapon;

        public void DestroyTower()
        {
            Destroy(gameObject);
            Plot.Tower = null;
            MapRenderer.Instance.InstanciatePlot(Plot);
        }

        public void AddElement(Element element)
        {
            if (Plot.Tower == null)
            {
                if (!GameManager.Instance.Game.Player.HasElement(element))
                {
                    return;
                }
                GameManager.Instance.Game.Player.RemoveElement(element);
                UiManager.Instance.ElementList.ReBuild();

                BuildingElements.Add(element.Uri);
                if (BuildingElements.Count >= 3)
                {
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
            var floating = MapRenderer.Instance.InstanciateFloatingElement(position, element, false);
            FloatingElements.Add(floating);
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
        }

        private void DisplayRangeSphere()
        {
            var visible = IsThisUnderMouse();

            if (rangeSphere == null)
            {
                rangeSphere = Instantiate(PrefabManager.Instance.GetPrefab("rangesphere"), transform);
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

            const float towerHeight = 2.5f;
            var ammoInfo = Plot.Tower.ShootAtTarget(ennemyInRange);
            ammoInfo.Origin = new Vector3(transform.position.x, transform.position.y + towerHeight, transform.position.z);

            var prefab = PrefabManager.Instance.GetPrefab(ammoInfo.PrefabName);
            var ammoObject = Instantiate(prefab, ammoInfo.Origin, Quaternion.identity);
            var ammoController = ammoObject.AddComponent<AmmoController>();
            ammoController.AmmoInfo = ammoInfo;
        }
    }
}
