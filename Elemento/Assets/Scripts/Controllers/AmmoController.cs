using Assets.Scripts.Controllers.Game;
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
            dir *= AmmoInfo.Speed * Time.fixedDeltaTime * /*TimeLineManager.Current.SimulationSpeed*/ 1;

            transform.position = transform.position + dir;
        }

        private void ApplyDamage()
        {
            if (AmmoInfo.Target == null)
            {
                return;
            }

            var monsterController = AmmoInfo.Target.GetComponent<MonsterController>();
            if (monsterController == null)
            {
                return;
            }

            monsterController.TakeDamages(AmmoInfo.Damage);
        }

        private bool ReachedTarget()
        {
            if (AmmoInfo.Target == null)
            {
                return true;
            }

            return Vector3.Distance(
                       AmmoInfo.Target.transform.position,
                       transform.position) <
                   reachedTargetThreshold;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if(AmmoInfo.Target != null)
                Gizmos.DrawLine(transform.position, AmmoInfo.Target.transform.position);
        }
    }
}