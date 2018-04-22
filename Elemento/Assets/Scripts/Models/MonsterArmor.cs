using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class MonsterArmor
    {
        [XmlAttribute]
        public DamageType DamageType;

        [XmlAttribute]
        public float FlatAmount;

        [XmlAttribute]
        public float Percent;
    }
}