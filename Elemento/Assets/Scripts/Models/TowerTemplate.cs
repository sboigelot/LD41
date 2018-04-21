using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class TowerTemplate : IPrototype
    {
        [XmlAttribute]
        public string Name;

        [XmlElement("Slot")]
        public List<TowerTemplateSlot> Slots;
    }
}