using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class MonsterPath
    {
        [XmlAttribute]
        public int Id;
        
        [XmlElement("Checkpoint")]
        public List<MonsterCheckpoint> MonsterCheckpoints;

        [XmlElement("DestinationPath")]
        public List<string> DestinationPaths;

        [XmlAttribute]
        public bool EndInStronghold;

        [XmlElement("BlokerPlot")]
        public List<string> BlokerPlots;
    }
}