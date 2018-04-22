using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ElementPrototypeReceipe : IPrototype
    {
        [XmlAttribute]
        public string Element1;

        [XmlAttribute]
        public string Element2;

        [XmlAttribute]
        public string ElementResult;
    }
}