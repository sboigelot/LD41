using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class MonsterSpawn
    {
        [XmlAttribute]
        public string MonsterPrototypeUri;

        [XmlAttribute]
        public int Count;

        [XmlIgnore]
        public int CountSpawned;
    }
}