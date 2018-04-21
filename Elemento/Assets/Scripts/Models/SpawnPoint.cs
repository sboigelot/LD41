using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<int> DestinationPaths;

        public MonsterPath GetAnyDestinationPath(Level level)
        {
            return
                level.MonsterPaths.FirstOrDefault(
                    mp => mp.Id == DestinationPaths[UnityEngine.Random.Range(0, DestinationPaths.Count)]);
        }
    }
}