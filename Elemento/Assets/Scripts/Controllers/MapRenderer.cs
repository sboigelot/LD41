using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Controllers.Game
{
    public class MapRenderer : MonoBehaviourSingleton<MapRenderer>
    {
        public float ModelScale = 2f;

        public List<GameObject> MapChildren;
        
        public void Resert()
        {
            if (MapChildren != null)
            {
                foreach (var go in MapChildren)
                {
                    Destroy(go);
                }
            }
            
            MapChildren = new List<GameObject>();
        }
        
        public void Build()
        {
            Resert();

            RenderTiles();
            RenderTowers();
            RenderSpawnPoints();
            RenderElementalNodes();
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

                    MapChildren.Add(tile);
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

        private void RenderElementalNodes()
        {
            var level = GameManager.Instance.Game.CurrentLevel;

            foreach (var elementalNode in level.ElementalNodes)
            {
                InstanciateElementalNode(elementalNode);
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

        public void InstanciateTower(TowerPlot plot)
        {
            var position = new Vector3(plot.X * ModelScale, GetHeight(plot.X, plot.Z), plot.Z * ModelScale);
            var prefab = PrefabManager.Instance.GetPrefab(plot.Tower.IsStronghold ? "stronghold": "tower");
            var instance = Instantiate(prefab, position, Quaternion.identity);

            var plotController = instance.AddComponent<TowerPlotController>();
            plotController.Plot = plot;

            if (!plot.Tower.IsStronghold)
            {
                var basePart = AddTowerPart(plot.Tower.BaseElementUri, TowerSlotType.Base, position, instance);
                var h = basePart.GetComponent<Renderer>().bounds.size.y;
                var bodyPart = AddTowerPart(plot.Tower.BodyElementUri, TowerSlotType.Body, position + new Vector3(0, h, 0), instance);
                h += bodyPart.GetComponent<Renderer>().bounds.size.y;
                plotController.Weapon = AddTowerPart(plot.Tower.WeaponElementUri, TowerSlotType.Weapon, position + new Vector3(0, h, 0), instance);
                plotController.WeaponCenter = h + plotController.Weapon.GetComponent<Renderer>().bounds.size.y / 2;
            }
            
            MapChildren.Add(instance);
        }

        private GameObject AddTowerPart(string elementuri, TowerSlotType slotType, Vector3 position, GameObject instance)
        {
            var elementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(elementuri);
            var stat = elementPrototype.ElementStats.FirstOrDefault(s => s.InSlot == slotType);
            string prefabName = "tower:base";
            if (stat == null)
            {
                Debug.LogWarningFormat("AddTowerPart Failed for {0} part not found {1}", elementPrototype.Name, slotType);
            }
            else
            {
                prefabName = stat.ModelPrefab;
            }
            var prefab = PrefabManager.Instance.GetPrefab(prefabName);
            return Instantiate(prefab, position, Quaternion.identity, instance.transform);
        }

        public void InstanciatePlot(TowerPlot plot)
        {
            var position = new Vector3(plot.X * ModelScale, GetHeight(plot.X, plot.Z), plot.Z * ModelScale);
            var instance = GameObject.Instantiate(PrefabManager.Instance.GetPrefab("towerplot"), position, Quaternion.identity, gameObject.transform);
            var plotController = instance.AddComponent<TowerPlotController>();
            plotController.Plot = plot;
            MapChildren.Add(instance);
        }


        private void InstanciateSpawnPoint(SpawnPoint spawnPoint)
        {
            var position = new Vector3(spawnPoint.X * ModelScale, GetHeight(spawnPoint.X, spawnPoint.Z), spawnPoint.Z * ModelScale);
            var spawn = GameObject.Instantiate(PrefabManager.Instance.GetPrefab("spawnpoint"), position, Quaternion.identity, gameObject.transform);
            MapChildren.Add(spawn);
        }


        private void InstanciateElementalNode(ElementalNode elementalNode)
        {
            var position = new Vector3(elementalNode.X * ModelScale, GetHeight(elementalNode.X, elementalNode.Z), elementalNode.Z * ModelScale);
            var node = GameObject.Instantiate(PrefabManager.Instance.GetPrefab("elementalnode"), position, Quaternion.identity, gameObject.transform);
            var nodeController = node.AddComponent<ElementalNodeController>();
            nodeController.ElementalNode = elementalNode;
            MapChildren.Add(node);
        }

        #region Gizmo
        public void OnDrawGizmos()
        {
            var game = GameManager.Instance.Game;
            if (game == null)
                return;

            var level = game.CurrentLevel;
            if (level == null)
                return;

            var paths = level.MonsterPaths;
            if (paths == null || !paths.Any())
                return;

            foreach (var monsterPath in paths)
            {
                DrawMonsterPathGizmmo(monsterPath);
            }
        }

        private void DrawMonsterPathGizmmo(MonsterPath monsterPath)
        {
            if (monsterPath.MonsterCheckpoints == null)
            {
                return;
            }

            MonsterCheckpoint prev = null;
            Vector3 prevPos = Vector3.zero;
            foreach (var checkpoint in monsterPath.MonsterCheckpoints)
            {
                Gizmos.color = Color.magenta;
                var ModelScale = MapRenderer.Instance.ModelScale;
                var position = new Vector3(checkpoint.X * ModelScale,
                    MapRenderer.Instance.GetHeight(checkpoint.X, checkpoint.Z) + 0.5f,
                    checkpoint.Z * ModelScale);
                Gizmos.DrawCube(position, new Vector3(1, 1, 1));
                
                if (prev != null)
                {
                    Gizmos.DrawLine(prevPos, position);
                }

                prev = checkpoint;
                prevPos = position;
            }
        }
        #endregion

        public GameObject InstanciateFloatingElement(Vector3 position, Element element, bool collectOnMouseClick)
        {
            var prefab = PrefabManager.Instance.GetPrefab("floatingelement");
            var floating = Instantiate(prefab, position, Quaternion.identity, transform);
            var floatingController = floating.GetComponent<FloatingElementController>();
            floatingController.ElementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(element.Uri);
            floatingController.Element = element;
            floatingController.CollectOnMouseClick = collectOnMouseClick;
            floatingController.Build();
            return floating;
        }
    }
}
