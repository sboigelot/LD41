using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers.Game
{
    public class ElementalNodeController : MonoBehaviour
    {
        public ElementalNode ElementalNode;

        public float NextSpawnTime;
        public int Spawned;
        public bool Done;

        public void Update()
        {
            var gameTime = GameManager.Instance.Game.GameTime;

            if (Done || 
                gameTime < ElementalNode.SpawnStart ||
                NextSpawnTime > gameTime)
            {
                return;
            }

            NextSpawnTime = gameTime + ElementalNode.SpawnDelay;
            Spawned++;

            var position = new Vector3(
                transform.position.x,
                transform.position.y + 1f,
                transform.position.z);
            var element = new Element
            {
                Count = ElementalNode.QuantityPerSpawn, Uri = ElementalNode.ElementUri
            };
            var floating = MapRenderer.Instance.InstanciateFloatingElement(position, element, true);

            Done = Spawned == ElementalNode.MaxSpawn;
        }
    }
}