using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class MonsterWaves
    {
        [XmlAttribute]
        public int SpawnpointId;

        [XmlAttribute]
        public float TriggerDeltaTime;

        [XmlAttribute]
        public float DeltaTimeBetweenSpawn;

        [XmlElement("Spawn")]
        public List<MonsterSpawn> Spawns;

        private float nextSpawnTime;
        public bool Done;

        public void Update(float deltaTime)
        {
            var gameTime = GameManager.Instance.Game.GameTime;

            if (gameTime < TriggerDeltaTime ||
                Done ||
                nextSpawnTime > gameTime ||
                Spawns == null)
            {
                return;
            }
            
            bool allDone = true;
            foreach (var monsterSpawn in Spawns)
            {
                if (monsterSpawn.CountSpawned < monsterSpawn.Count)
                {
                    allDone = false;
                    var monsterPrototype =
                        PrototypeManager.Instance.GetPrototype<MonsterPrototype>(monsterSpawn.MonsterPrototypeUri);

                    if (monsterPrototype == null)
                    {
                        Done = true;
                        Debug.LogError("MonsterWave coudn't find the monster prototype: " +
                                       monsterSpawn.MonsterPrototypeUri);
                        return;
                    }

                    GameManager.Instance.SpawnMonster(SpawnpointId, monsterPrototype);
                    monsterSpawn.CountSpawned++;
                    nextSpawnTime = gameTime + DeltaTimeBetweenSpawn;
                    break;
                }
            }
            Done = allDone;
        }
    }
}