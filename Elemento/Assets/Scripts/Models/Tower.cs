using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Tower
    {
        [XmlAttribute]
        public string Id;

        [XmlAttribute]
        public string Prototype;
    }
}