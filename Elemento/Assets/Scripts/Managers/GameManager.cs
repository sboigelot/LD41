using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controllers.Game;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Controls.ContextualMenu;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>, IContextualMenuItemInfoProvider
    {
        public Game Game;
        public PrototypeManager PrototypeManager;

        public List<GameObject> CurrentMonsters;

        public Sprite DefaultContextualMenuItemSprite;

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
        
        public void SpawnMonster(int spawnpointId, string monsterPrototypeUri)
        {
            var monsterPrefab = PrefabManager.Instance.GetPrefab(monsterPrototypeUri);
            var spawnPoint = Game.CurrentLevel.SpawnPoints.FirstOrDefault(s => s.Id == spawnpointId);

            if (monsterPrefab == null || spawnPoint == null)
            {
                Debug.LogWarning("Level.SpawnMonster ccoun't find monster prefab " +
                    monsterPrototypeUri +
                    "or spawnpoint " +
                    spawnpointId);
                return;
            }

            var ModelScale = MapRenderer.Instance.ModelScale;

            var position = new Vector3(spawnPoint.X * ModelScale, MapRenderer.Instance.GetHeight(spawnPoint.X, spawnPoint.Z), spawnPoint.Z * ModelScale);
            var monster = GameObject.Instantiate(PrefabManager.Instance.GetPrefab(monsterPrototypeUri), position, Quaternion.identity);
            var monsterController = monster.AddComponent<MonsterController>();
            monsterController.CurrentPath = spawnPoint.GetAnyDestinationPath(Game.CurrentLevel);
            CurrentMonsters.Add(monster);
        }

        private bool RaycastAll(Ray ray, out RaycastHit hit)
        {
            // LayerMask layermask = new LayerMask {value = terrainGameObject.layer};

            hit = new RaycastHit();
            if (!Physics.Raycast(ray, out hit, 1000/*, layermask*/))
            {
                Debug.LogWarning("Raycast failed");
                return false;
            }
            return true;
        }

        public virtual IEnumerable<ContextualMenuItemInfo> GetContextualMenuInfo()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
      
            if (RaycastAll(ray, out hit))
            yield return new ContextualMenuItemInfo
            {
                Image = DefaultContextualMenuItemSprite,
                IsEnable = () => true,
                Name = "Item 1",
                TooltipText = "Item 1",
                OnClick = LogClick
            };

            yield return new ContextualMenuItemInfo
            {
                Image = DefaultContextualMenuItemSprite,
                IsEnable = () => true,
                Name = "Item 2",
                TooltipText = "Item 2",
                OnClick = LogClick
            };

            yield return new ContextualMenuItemInfo
            {
                Image = DefaultContextualMenuItemSprite,
                IsEnable = () => true,
                Name = "Item 3",
                TooltipText = "Item 3",
                OnClick = LogClick
            };
        }

        private void LogClick(Scripts.ContextualMenu menu, GameObject instanciator, Vector3 position)
        {
            Debug.Log("click on ctx menu item");
        }
    }
}
