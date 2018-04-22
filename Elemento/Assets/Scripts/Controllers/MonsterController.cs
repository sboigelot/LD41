using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers.Game
{
    public class MonsterController : MonoBehaviour
    {
        public MonsterPrototype MonsterPrototype;
        public MonsterPath CurrentPath;

        public int CurrentCheckpointIndex;
        public float Speed = 0.7f;
        public float DestinationReachedThreshold = 0.4f;
        public float Hp;

        private CharacterController controller;

        public void Start()
        {
            controller = gameObject.AddComponent<CharacterController>();
            controller.height = 0.1f;
            controller.radius = 0.1f;
            controller.detectCollisions = false;
        }

        public void ReachDestination()
        {
            if (CurrentPath.EndInStronghold)
            {
                GameManager.Instance.StrongholdDamage(MonsterPrototype.DamageOnStronghold);
                CurrentPath = null;
                GameObject.Destroy(gameObject);
            }
            else
            {
                CurrentPath = CurrentPath.GetAnyDestinationPath(GameManager.Instance.Game.CurrentLevel);
                CurrentCheckpointIndex = 0;
            }
        }

        public void FixedUpdate()
        {
            if (CurrentPath == null)
            {
                //We have no path to move after yet
                return;
            }

            if (CurrentPath.MonsterCheckpoints == null)
            {
                Debug.LogWarningFormat("CurrentPath has null CurrentPath.MonsterCheckpoints: {0}", CurrentPath.Id);
                return;
            }

            if (CurrentCheckpointIndex >= CurrentPath.MonsterCheckpoints.Count)
            {
                ReachDestination();
                return;
            }

            var d = CurrentPath.MonsterCheckpoints[CurrentCheckpointIndex];

            var ModelScale = MapRenderer.Instance.ModelScale;
            var currentDestination = new Vector3(
                d.X * ModelScale,
                MapRenderer.Instance.GetHeight(d.X, d.Z),
                d.Z * ModelScale);

            var distance = Vector3.Distance(transform.position, currentDestination);

            //Check if we are close enough to the next waypoint
            //If we are, proceed to follow the next waypoint
            if (!(distance > DestinationReachedThreshold))
            {
                CurrentCheckpointIndex++;
                return;
            }
            
            //rotate the model
            Quaternion newRotation = 
                Quaternion.LookRotation(currentDestination - transform.position,
                Vector3.forward);
            newRotation.x = 0;
            newRotation.z = 0;
            transform.rotation = newRotation;

            //Direction to the next waypoint
            Vector3 dir = (currentDestination - transform.position).normalized;
            dir *= Speed * Time.fixedDeltaTime * /*TimeLineManager.Current.SimulationSpeed*/ 1;
            //dir.y = 0f;
            controller.Move(dir);
        }

        public void OnDestroy()
        {
            GameManager.Instance.UnRegisterMonster(this);
        }

        public void TakeDamages(Dictionary<DamageType, float> damages)
        {
            foreach (var damage in damages)
            {
                TakeDamage(damage.Key, damage.Value);
            }
        }

        public void TakeDamage(DamageType damageType, float amount)
        {
            var realDamage = CompensateArmor(damageType, amount);
            Hp -= realDamage;

            if (Hp < 0)
            {
                Die();
            }
        }

        private float CompensateArmor(DamageType damageType, float amount)
        {
            if (MonsterPrototype == null ||
                MonsterPrototype.Armors == null)
            {
                return amount;
            }

            var armor = MonsterPrototype.Armors.FirstOrDefault(a => a.DamageType == damageType);
            if (armor == null)
            {
                return amount;
            }

            amount -= armor.FlatAmount;
            amount -= (armor.Percent / 100f * amount);

            return amount;
        }

        private void Die()
        {
            Destroy(gameObject);
            if (MonsterPrototype.Loots == null)
            {
                return;
            }

            foreach (var loot in MonsterPrototype.Loots)
            {
                var position = new Vector3(
                  transform.position.x,
                  transform.position.y + 1f,
                  transform.position.z);
                MapRenderer.Instance.InstanciateFloatingElement(position, loot, true);
            }
        }
    }
}
