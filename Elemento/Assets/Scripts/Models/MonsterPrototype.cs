using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class MonsterPrototype : IPrototype
    {
        [XmlAttribute]
        public string PrefabName;

        [XmlAttribute]
        public float Speed;

        [XmlAttribute]
        public float Hp;

        [XmlAttribute]
        public float DamageOnStronghold;

        [XmlElement("Armor")]
        public List<MonsterArmor> Armors;

        [XmlElement("Loot")]
        public List<Element> Loots;
    }
}