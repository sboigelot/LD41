using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Element
    {
        [XmlAttribute]
        public string Uri;

        [XmlAttribute]
        public int Count;
    }
}