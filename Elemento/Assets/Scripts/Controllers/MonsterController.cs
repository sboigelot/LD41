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
                //TODO
                Debug.Log("Monster reached stronghold - not implemented");
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
    }
}
