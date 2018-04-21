using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class TowerPlot
    {
        [XmlAttribute]
        public int Id;

        [XmlAttribute]
        public float X;

        [XmlAttribute]
        public float Y;

        [XmlAttribute]
        public float Z;
        

        [XmlAttribute]
        public bool Editable;

        [XmlElement("Tower")]
        public Tower Tower;
    }
}