using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class MonsterCheckpoint
    {
        [XmlAttribute]
        public int Id;

        [XmlAttribute]
        public int X;
        
        [XmlAttribute]
        public int Z;
    }
}