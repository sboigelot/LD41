using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class TowerPlotController : MonoBehaviour
    {
        public TowerPlot Plot;

        private GameObject rangeSphere;

        public void DestroyTower()
        {
            
        }

        public void AddElement(Element element)
        {
            
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
                rangeSphere.transform.localScale = new Vector3(range, range, range);
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
                Debug.LogWarning("Raycast failed");
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
            
            if (!Plot.Tower.ReadyToShoot)
            {
                return;
            }

            var ennemyInRange = Plot.Tower.FindTarget(transform.position);
            if (ennemyInRange == null)
            {
                return;
            }

            const float towerHeight = 1f;
            var ammoInfo = Plot.Tower.ShootAtTarget(ennemyInRange);
            ammoInfo.Origin = new Vector3(transform.position.x, transform.position.y + towerHeight, transform.position.z);

            var prefab = PrefabManager.Instance.GetPrefab(ammoInfo.PrefabName);
            var ammoObject = Instantiate(prefab, ammoInfo.Origin, Quaternion.identity);
            var ammoController = ammoObject.AddComponent<AmmoController>();
            ammoController.AmmoInfo = ammoInfo;
        }
    }
}
