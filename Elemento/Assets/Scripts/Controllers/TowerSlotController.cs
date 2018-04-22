using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class TowerPlotController : MonoBehaviour
    {
        public TowerPlot Plot;

        public void DestroyTower()
        {
            
        }

        public void AddElement(Element element)
        {
            
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
