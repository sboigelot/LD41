﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controllers;
using Assets.Scripts.Controllers.Game;
using Assets.Scripts.Controllers.UI;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Controls.ContextualMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public Game Game;
        public PrototypeManager PrototypeManager;

        public List<GameObject> CurrentMonsters;

        public Slider HpSlider;

        public Text WaveText;

        public QuitDialogController GameWonPanel;
        public QuitDialogController GameOverPanel;
        public float SoundVolume = 0.5f;

        public void Start()
        {
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
            StartCoroutine(PreloadSprites());
            GameOverPanel.gameObject.SetActive(false);
            GameWonPanel.gameObject.SetActive(false);

            CurrentMonsters = new List<GameObject>();
            Game = new Game();
            Game.Initialize();

            var levelHolder = FindObjectOfType<LevelDataHolder>();
            Game.CurrentLevel = PrototypeManager.GetPrototype<Level>(levelHolder!=null? levelHolder.Level.Uri : "level:tutorial");

            LoadPlayer();

            MapRenderer.Instance.Build();

            if (HpSlider != null)
            {
                HpSlider.maxValue = Game.CurrentLevel.StrongholdHp;
                HpSlider.minValue = 0;
                HpSlider.value = HpSlider.maxValue;
            }
        }

        private IEnumerator PreloadSprites()
        {
            var allImages = PrototypeManager
                .Instance
                .GetAllPrototypes<ElementPrototype>()
                .Select(a => a.SpritePath)
                .ToList();

            foreach (var path in allImages)
            {
                //Debug.Log("Preloading image: " + path);
                StartCoroutine(SpriteManager.Set(sprite => { }, "Images/Elements", path));
                yield return null;
            }

            var allLevelPreviews = PrototypeManager
                .Instance
                .GetAllPrototypes<Level>()
                .Select(a => a.PreviewPicPath)
                .ToList();

            foreach (var path in allLevelPreviews)
            {
                //Debug.Log("Preloading image: " + path);
                StartCoroutine(SpriteManager.Set(sprite => { }, "Data/Levels", path));
                yield return null;
            }
        }

        private bool alreadyEnded = false;
        public void EndGame(bool win)
        {
            if (alreadyEnded)
            {
                return;
            }
            alreadyEnded = true;
            Game.Paused = true;
            if (win)
            {
                SoundController.Instance.PlaySound(SoundController.Instance.GameWon);
                GameWonPanel.OpenDialog();
            }
            else
            {
                SoundController.Instance.PlaySound(SoundController.Instance.GameOver);
                GameOverPanel.OpenDialog();
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
            SoundController.Instance.PlaySound(SoundController.Instance.Popup);
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
            monsterController.MonsterPrototype = monsterPrototype;
            monsterController.CurrentPath = spawnPoint.GetAnyDestinationPath(Game.CurrentLevel);
            monsterController.Speed = monsterPrototype.Speed;
            monsterController.Hp = monsterPrototype.Hp;
            monsterController.AddHealthBar();
            CurrentMonsters.Add(monster);
        }

        public void StrongholdDamage(float amount)
        {
            Game.CurrentLevel.StrongholdHp -= amount;

            if(HpSlider != null)
            {
                HpSlider.value = Game.CurrentLevel.StrongholdHp;
            }

            if (Game.CurrentLevel.StrongholdHp <= 0)
            {
                EndGame(false);
            }
        }

        public void Pause(bool value)
        {
            Game.Paused = value;
        }

        public void SpeedUp(bool value)
        {
            Game.GameSpeed = value ? 2 : 1;
        }

        public void UpdateNextWaveInfo(int waveIndex, int totalWaves, int nextWaveTimer)
        {
            if (waveIndex == 0)
            {
                WaveText.text = string.Format("{0} Waves starts in {1} sec", totalWaves, nextWaveTimer);
                return;
            }

            if (waveIndex == totalWaves)
            {
                WaveText.text = string.Format("Wave {0} of {1}, last wave!!!", waveIndex, totalWaves);
                return;
            }

            WaveText.text = string.Format("Wave {0} of {1}, Next in {2} sec", waveIndex, totalWaves, nextWaveTimer);
        }
    }
}
