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
        public float X;

        [XmlAttribute]
        public float Y;

        [XmlAttribute]
        public float Z;
    }
}