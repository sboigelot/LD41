using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class SpawnPoint
    {
        [XmlAttribute]
        public int Id;

        [XmlAttribute]
        public float X;

        [XmlAttribute]
        public float Y;

        [XmlAttribute]
        public float Z;

        [XmlElement("DestinationPath")]
        public List<string> DestinationPaths;
    }
}