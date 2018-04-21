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
        public float ModelScale = 2f;

        public List<GameObject> CurrentTiles;

        public Dictionary<string, GameObject> CurrentTowers;

        public Dictionary<string, GameObject> CurrentSpawnPoint;

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

            if (CurrentSpawnPoint != null)
            {
                foreach (var go in CurrentSpawnPoint.Values)
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
            CurrentSpawnPoint = new Dictionary<string, GameObject>();
        }

        public void Start()
        {
        }

        public void Build()
        {
            Resert();

            RenderTiles();
            RenderTowers();
            RenderSpawnPoints();
        }

        private void RenderTiles()
        {
            var level = GameManager.Instance.Game.CurrentLevel;
            var tiles = level.Tiles;

            for (int x = 0; x < level.SizeX; x++)
            {
                for (int z = 0; z < level.SizeZ; z++)
                {
                    var position = new Vector3(x * ModelScale, -.3f, z * ModelScale);
                    var tile = new GameObject("Tile("+x+","+z+")");
                    tile.transform.position = position;
                    tile.transform.SetParent(gameObject.transform);
                    var tileRenderer = tile.AddComponent<TileRenderer>();
                    tileRenderer.X = x;
                    tileRenderer.Z = z;
                    tileRenderer.Build(level, tiles);

                    CurrentTiles.Add(tile);
                }
            }
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
                else
                {
                    InstanciatePlot(plot);
                }
            }
        }

        public void RenderSpawnPoints()
        {
            var level = GameManager.Instance.Game.CurrentLevel;

            foreach (var spawnPoint in level.SpawnPoints)
            {
                InstanciateSpawnPoint(spawnPoint);
            }
        }

        public float GetHeight(int x, int z)
        {
            var level = GameManager.Instance.Game.CurrentLevel;
            float height = 0f;
            if (!string.IsNullOrEmpty(level.HeightmapString))
            {
                height = level.Heightmap[x][z];
                if (Math.Abs(height) < 0.1f)
                {
                    return 0f;
                }
                height -= 1f;
            }

            return height;
        }

        private void InstanciateTower(TowerPlot plot)
        {
            var position = new Vector3(plot.X * ModelScale, GetHeight(plot.X, plot.Z), plot.Z * ModelScale);
            var tile = GameObject.Instantiate(PrefabManager.Instance.GetPrefab("tower"), position, Quaternion.identity, gameObject.transform);
            CurrentTiles.Add(tile);
        }

        private void InstanciatePlot(TowerPlot plot)
        {
            var position = new Vector3(plot.X * ModelScale, GetHeight(plot.X, plot.Z), plot.Z * ModelScale);
            var tile = GameObject.Instantiate(PrefabManager.Instance.GetPrefab("towerplot"), position, Quaternion.identity, gameObject.transform);
            CurrentTiles.Add(tile);
        }


        private void InstanciateSpawnPoint(SpawnPoint spawnPoint)
        {
            var position = new Vector3(spawnPoint.X * ModelScale, GetHeight(spawnPoint.X, spawnPoint.Z), spawnPoint.Z * ModelScale);
            var tile = GameObject.Instantiate(PrefabManager.Instance.GetPrefab("spawnpoint"), position, Quaternion.identity, gameObject.transform);
            CurrentTiles.Add(tile);
        }
    }
}
