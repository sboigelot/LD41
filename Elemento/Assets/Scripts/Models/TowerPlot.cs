using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class TowerPlot
    {
        [XmlAttribute]
        public int Id;

        [XmlAttribute]
        public int X;
        
        [XmlAttribute]
        public int Z;
        

        [XmlAttribute]
        public bool Editable;

        [XmlElement("Tower")]
        public Tower Tower;

        public void Update(float deltaTime)
        {
            if (Tower == null)
            {
                return;
            }

            Tower.Update(deltaTime);
        }
    }
}