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

            Plot.Tower.Update(transform.position);
        }
    }
}
