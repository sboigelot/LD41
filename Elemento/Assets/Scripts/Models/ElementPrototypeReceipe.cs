using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ElementPrototypeReceipe
    {
        [XmlElement("Ingredient")]
        public List<string> Ingredients;
    }
}