using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers.Game
{
    public class MonsterController : MonoBehaviour
    {
        public MonsterPrototype MonsterPrototype;
        public MonsterPath CurrentPath;
        public int CurrentCheckpointIndex;
        public float Speed = 0.35f;
        public float DestinationReachedThreshold = 0.40f;

        private CharacterController controller;

        public void Start()
        {
            controller = gameObject.AddComponent<CharacterController>();
        }

        public void ReachDestination()
        {
            //TODO
        }

        public void FixedUpdate()
        {
            if (CurrentPath == null)
            {
                //We have no path to move after yet
                return;
            }

            if (CurrentCheckpointIndex >= CurrentPath.MonsterCheckpoints.Count)
            {
                ReachDestination();
                CurrentPath = null;
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
    }
}
