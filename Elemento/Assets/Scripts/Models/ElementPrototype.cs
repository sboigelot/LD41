using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ElementPrototype : IPrototype
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string SpritePath;
        
        [XmlElement("Stat")]
        public List<ElementPrototypeStat> ElementStats;

        [XmlAttribute]
        public string UnlockTowerTemplate;
    }
}