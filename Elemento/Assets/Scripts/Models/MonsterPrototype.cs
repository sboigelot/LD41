using System;
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
    }
}