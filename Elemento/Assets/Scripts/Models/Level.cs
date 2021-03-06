﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Assets.Scripts.Controllers.Game;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Level : IPrototype
    {
        public string Uri;

        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string PreviewPicPath;

        [XmlAttribute]
        public int Order;

        [XmlAttribute]
        public float StrongholdHp;
        
        [XmlAttribute]
        public int SizeX;

        [XmlAttribute]
        public int SizeZ;

        [XmlElement("StartingElement")]
        public List<Element> StartingElements;

        [XmlElement]
        public string TileString;

        private int[][] tiles;

        [XmlIgnore]
        public int[][] Tiles
        {
            get
            {
                if (tiles != null)
                {
                    return tiles;
                }

                if (string.IsNullOrEmpty(TileString))
                {
                    for (int nx = 0; nx < SizeX; nx++)
                    {
                        tiles[nx] = new int[SizeZ];
                        for (int nz = 0; nz < SizeZ; nz++)
                        {
                            tiles[nx][nz] = 1;
                        }
                    }
                    return tiles;
                }

                tiles = new int[SizeX][];

                var split = TileString.Replace(Environment.NewLine,"").Trim().Split(',').ToList();

                var x = -1;
                var z = 0;

                foreach (var tileChar in split)
                {
                    if (z == 0)
                    {
                        x++;
                        tiles[x] = new int[SizeZ];
                    }

                    tiles[x][z] = int.Parse(tileChar);

                    z = ++z % SizeZ;
                }

                return tiles;
            }
        }

        private float[][] heightmap;

        [XmlElement]
        public string HeightmapString;

        [XmlIgnore]
        public float[][] Heightmap
        {
            get
            {
                if (heightmap != null)
                {
                    return heightmap;
                }

                heightmap = new float[SizeX][];

                if (string.IsNullOrEmpty(HeightmapString))
                {
                    for (int nx = 0; nx < SizeX; nx++)
                    {
                        heightmap[nx] = new float[SizeZ];
                        for (int nz = 0; nz < SizeZ; nz++)
                        {
                            heightmap[nx][nz] = 1;
                        }
                    }
                    return heightmap;
                }

                var split = HeightmapString.Replace(Environment.NewLine, "").Trim().Split(',').ToList();

                var x = -1;
                var z = 0;

                foreach (var tileChar in split)
                {
                    if (z == 0)
                    {
                        x++;
                        heightmap[x] = new float[SizeZ];
                    }

                    heightmap[x][z] = float.Parse(tileChar);

                    z = ++z % SizeZ;
                }

                return heightmap;
            }
        }


        [XmlElement("TowerPlot")]
        public List<TowerPlot> TowerPlots;
        
        [XmlElement("SpawnPoint")]
        public List<SpawnPoint> SpawnPoints;

        [XmlElement("MonsterPath")]
        public List<MonsterPath> MonsterPaths;

        [XmlElement("MonsterWave")]
        public List<MonsterWaves> MonsterWaves;

        [XmlElement("ElementalNode")]
        public List<ElementalNode> ElementalNodes;

        [XmlElement("TutorialStep")]
        public List<TutorialStep> TutorialSteps;

        [XmlIgnore]
        public int TutorialIndex = -1;
        
        public void Update(float deltaTime)
        {
            TutorialManager.Instance.UpdateTutorial(this);
            UpdateMonsterWaves(deltaTime);
        }

        private void UpdateMonsterWaves(float deltaTime)
        {
            var allEnded = true;
            if (MonsterWaves != null)
            {
                var index = 0;
                int next = -1;
                foreach (var monsterWave in MonsterWaves)
                {
                    monsterWave.Update(deltaTime);
                    if (monsterWave.TriggerDeltaTime < GameManager.Instance.Game.GameTime)
                    {
                        index++;
                    }
                    else
                    {
                        if (next == -1)
                        {
                            next = (int)(monsterWave.TriggerDeltaTime - GameManager.Instance.Game.GameTime);
                        }
                    }
                    allEnded &= monsterWave.Done;
                }
                GameManager.Instance.UpdateNextWaveInfo(index, MonsterWaves.Count, next);
            }

            if (allEnded)
            {
                if (GameManager.Instance.CurrentMonsters.Count == 0)
                {
                    GameManager.Instance.EndGame(true);
                }
            }
        }
    }
}