using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

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
        public string ModelName;

        [XmlAttribute]
        public int SizeX;

        [XmlAttribute]
        public int SizeZ;

        [XmlElement]
        public string TileString;

        [XmlIgnore]
        public int[][] Tiles
        {
            get
            {
                var tiles = new int[SizeX][];

                var split = TileString.Replace(Environment.NewLine,"").Trim().Split(';').ToList();

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

        [XmlElement("TowerPlot")]
        public List<TowerPlot> TowerPlots;
        
        [XmlElement("SpawnPoint")]
        public List<SpawnPoint> SpawnPoints;

        [XmlElement("MonsterPath")]
        public List<MonsterPath> MonsterPaths;

        [XmlElement("MonsterWave")]
        public List<MonsterWaves> MonsterWaves;
    }
}