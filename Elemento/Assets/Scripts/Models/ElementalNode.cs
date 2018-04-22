using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ElementalNode
    {
        [XmlAttribute]
        public int X;

        [XmlAttribute]
        public int Z;

        [XmlAttribute]
        public string ElementUri;

        [XmlAttribute]
        public int QuantityPerSpawn;

        [XmlAttribute]
        public float SpawnDelay;

        [XmlAttribute]
        public float SpawnStart;

        [XmlAttribute]
        public int MaxSpawn;
    }
}