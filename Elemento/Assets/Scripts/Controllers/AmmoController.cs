using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class AmmoController : MonoBehaviour
    {
        public AmmoSpawnInfo AmmoInfo;

        private float reachedTargetThreshold = 0.40f;

        public void FixedUpdate()
        {
            if (ReachedTarget())
            {
                ApplyDamage();
                Destroy(this.gameObject);
                return;
            }

            MoveToTarget();
        }

        private void MoveToTarget()
        {
            Quaternion newRotation =
                Quaternion.LookRotation(AmmoInfo.Target.transform.position - transform.position,
                Vector3.forward);
            newRotation.x = 0;
            newRotation.z = 0;
            transform.rotation = newRotation;

            Vector3 dir = (AmmoInfo.Target.transform.position - transform.position).normalized;
            dir *= (AmmoInfo.Speed/10f) * Time.fixedDeltaTime * /*TimeLineManager.Current.SimulationSpeed*/ 1;

            transform.Translate(dir);
        }

        private void ApplyDamage()
        {
            
        }

        private bool ReachedTarget()
        {
            return Vector3.Distance(
                       AmmoInfo.Target.transform.position,
                       transform.position) >
                   reachedTargetThreshold;
        }
    }
}