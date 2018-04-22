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
        public float DamageAmount;
        
        [XmlAttribute]
        public float SpeedBonus;

        [XmlAttribute]
        public float RangeBonus;

        [XmlAttribute]
        public float AmmoSpeedBonus;

        [XmlAttribute]
        public string AmmoPrefabName = "defaultammo";

        [XmlAttribute]
        public string ModelPrefab;
    }
}