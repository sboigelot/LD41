using System;
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
        [XmlAttribute]
        public string Name;

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

        public void Update(float deltaTime)
        {
            UpdateMonsterWaves(deltaTime);
        }

        private void UpdateMonsterWaves(float deltaTime)
        {
            if (MonsterWaves != null)
            {
                foreach (var monsterWaves in MonsterWaves)
                {
                    monsterWaves.Update(deltaTime);
                }
            }
        }
    }
}