using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<int> DestinationPaths;

        [XmlAttribute]
        public bool EndInStronghold;

        [XmlElement("BlockerPlot")]
        public List<string> BlockerPlots;
        
        public MonsterPath GetAnyDestinationPath(Level level)
        {
            var pathId = DestinationPaths[UnityEngine.Random.Range(0, DestinationPaths.Count)];
            Debug.LogFormat("Request next path provided id {0}", pathId);
            if (!level.MonsterPaths.Any(mp => mp.Id == pathId))
            {
                Debug.LogErrorFormat("Request next path provided id {0} but this id is not found", pathId);
            }
            return level.MonsterPaths.FirstOrDefault(mp => mp.Id == pathId);
        }
    }
}