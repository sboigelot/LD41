using System;
using System.Xml;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ElementPrototypeStat
    {
        [XmlAttribute]
        public TowerSlotType InSlot;

        [XmlAttribute]
        public DamageType DamageType;

        [XmlAttribute]
        public int DamageAmount;
        
        [XmlAttribute]
        public int SpeedBonus;

        [XmlAttribute]
        public int RangeBonus;

        [XmlAttribute]
        public int AmmoSpeedBonus;
    }
}