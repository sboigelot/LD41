using System;
using System.Collections.Generic;
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