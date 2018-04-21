using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers.Game
{
    public class MapRenderer : MonoBehaviourSingleton<MapRenderer>
    {
        public List<GameObject> CurrentTiles;

        public Dictionary<string, GameObject> CurrentTowers;

        public Dictionary<string, GameObject> CurrentMonsters;

        public void Resert()
        {
            if (CurrentTiles != null)
            {
                foreach (var go in CurrentTiles)
                {
                    Destroy(go);
                }
            }

            if (CurrentTowers != null)
            {
                foreach (var go in CurrentTowers.Values)
                {
                    Destroy(go);
                }
            }

            if (CurrentMonsters != null)
            {
                foreach (var go in CurrentMonsters.Values)
                {
                    Destroy(go);
                }
            }

            CurrentTiles = new List<GameObject>();
            CurrentTowers = new Dictionary<string, GameObject>();
            CurrentMonsters = new Dictionary<string, GameObject>();
        }

        public void Start()
        {
        }

        public void Build()
        {
            Resert();

            RenderTiles();
            RenderTowers();
        }

        private void RenderTiles()
        {
            var level = GameManager.Instance.Game.CurrentLevel;

            for (int x = 0; x < level.SizeX; x++)
            {
                for (int z = 0; z < level.SizeZ; z++)
                {
                    InstanciateTile(x, z);
                }
            }
        }

        private void InstanciateTile(int x, int z)
        {
            var position = new Vector3(x, 0, z);
            var tile = GameObject.Instantiate(PrefabManager.Instance.TilePrefab, position, Quaternion.identity, gameObject.transform);
            CurrentTiles.Add(tile);
        }

        private void RenderTowers()
        {
            var level = GameManager.Instance.Game.CurrentLevel;

            foreach (var plot in level.TowerPlots)
            {
                if (plot.Tower != null)
                {
                    InstanciateTower(plot);
                }
            }

        }

        private void InstanciateTower(TowerPlot plot)
        {
            var position = new Vector3(plot.X, plot.Y, plot.Z);
            var tile = GameObject.Instantiate(PrefabManager.Instance.TowerPrefab, position, Quaternion.identity, gameObject.transform);
            CurrentTiles.Add(tile);
        }
    }
}
