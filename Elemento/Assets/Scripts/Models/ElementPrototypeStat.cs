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
        public bool IsArmorBonus;

        [XmlAttribute]
        public bool IsDamageBonus;

        [XmlAttribute]
        public DamageType DamageType;

        [XmlAttribute]
        public int Amount;

        [XmlAttribute]
        public int HpBonus;

        [XmlAttribute]
        public int SpeedBonus;

        [XmlAttribute]
        public int RangeBonus;
    }
}