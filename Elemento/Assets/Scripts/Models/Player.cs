using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Player : IPrototype
    {
        [XmlElement("UnlockedTowerTemplate")]
        public List<string> UnlockedTowerTemplates;

        public List<Element> Elements;

        public List<string> KnownElements;

        public Player()
        {
            Elements = new List<Element>();
            KnownElements = new List<string>();
            UnlockedTowerTemplates = new List<string>();
        }

        public void AddElement(Element e)
        {
            var existing = Elements.FirstOrDefault(el => el.Uri == e.Uri);
            if (existing == null)
            {
                Elements.Add(e);
                return;
            }

            existing.Count += e.Count;
        }

        public bool RemoveElement(Element e)
        {
            if (!HasElement(e))
            {
                return false;
            }

            var existing = Elements.First(el => el.Uri == e.Uri);
            existing.Count += e.Count;
            return true;
        }

        public bool HasElement(Element e)
        {
            var existing = Elements.FirstOrDefault(el => el.Uri == e.Uri);
            if (existing == null)
            {
                return false;
            }

            if (existing.Count >= e.Count)
            {
                return false;
            }
            
            return true;
        }
    }
}