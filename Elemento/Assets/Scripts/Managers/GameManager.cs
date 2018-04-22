﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controllers.Game;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Controls.ContextualMenu;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public Game Game;
        public PrototypeManager PrototypeManager;

        public List<GameObject> CurrentMonsters;

        public void Start()
        {
            DontDestroyOnLoad(gameObject);

            //make the prototypeManager visible in unity inspector
            PrototypeManager = PrototypeManager.Instance;
            StartCoroutine(PrototypeManager.LoadPrototypes(NewGame));
        }

        private void LoadPlayer()
        {
            Debug.Log("GameManager.LoadPlayer is mocked for now");
            Game.Player = new Player();

            foreach (var element in Game.CurrentLevel.StartingElements)
            {
                Game.Player.AddElement(element);
            }

            UiManager.Instance.ElementList.ReBuild();
        }

        public void NewGame()
        {
            // StartScreenPanel.SetActive(true);
            // GameOverPanel.SetActive(false);
            // GameWonPanel.SetActive(false);

            CurrentMonsters = new List<GameObject>();
            Game = new Game();
            Game.Initialize();
            Game.CurrentLevel = PrototypeManager.GetPrototype<Level>("level:level_test");
            LoadPlayer();
            MapRenderer.Instance.Build();
        }

        public void EndGame(bool win)
        {
            Game.Paused = true;
            if (win)
            {
                // GameWonPanel.GetComponent<EndGamePanel>().Open();
            }
            else
            {
                // GameOverPanel.GetComponent<EndGamePanel>().Open();
            }
        }

        public void Update()
        {
            if (Game != null)
            {
                Game.Update(Time.deltaTime);
            }
        }

        public void UnRegisterMonster(MonsterController monster)
        {
            if (CurrentMonsters != null && CurrentMonsters.Contains(monster.gameObject))
            {
                CurrentMonsters.Remove(monster.gameObject);
            }
        }
        
        public void SpawnMonster(int spawnpointId, MonsterPrototype monsterPrototype)
        {
            var monsterPrefab = PrefabManager.Instance.GetPrefab(monsterPrototype.PrefabName);
            var spawnPoint = Game.CurrentLevel.SpawnPoints.FirstOrDefault(s => s.Id == spawnpointId);

            if (monsterPrefab == null || spawnPoint == null)
            {
                Debug.LogWarning("Level.SpawnMonster ccoun't find monster prefab " +
                    monsterPrototype.PrefabName +
                    "or spawnpoint " +
                    spawnpointId);
                return;
            }
            
            var ModelScale = MapRenderer.Instance.ModelScale;

            var position = new Vector3(spawnPoint.X * ModelScale, MapRenderer.Instance.GetHeight(spawnPoint.X, spawnPoint.Z), spawnPoint.Z * ModelScale);
            var monster = GameObject.Instantiate(monsterPrefab, position, Quaternion.identity);
            var monsterController = monster.AddComponent<MonsterController>();
            monsterController.CurrentPath = spawnPoint.GetAnyDestinationPath(Game.CurrentLevel);
            monsterController.Speed = monsterPrototype.Speed;
            CurrentMonsters.Add(monster);
        }
    }
}
