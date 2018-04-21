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
        public int X;
        
        [XmlAttribute]
        public int Z;

        [XmlElement("DestinationPath")]
        public List<string> DestinationPaths;
    }
}