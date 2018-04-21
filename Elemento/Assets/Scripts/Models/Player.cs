using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Player : IPrototype
    {
        [XmlElement("UnlockedTowerTemplate")]
        public List<string> UnlockedTowerTemplates;

        public List<Element> Elements;

        public List<string> KnownElements;
    }
}