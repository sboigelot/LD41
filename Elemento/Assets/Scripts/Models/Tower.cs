using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Tower
    {
        [XmlAttribute]
        public string Id;

        [XmlAttribute]
        public string Prototype;

        public void Update(float deltaTime)
        {
            // TODO
            //Debug.LogWarning("Level.SpawnMonster Not Implemented");
        }
    }
}