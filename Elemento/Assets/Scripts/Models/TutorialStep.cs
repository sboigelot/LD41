using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class TutorialStep
    {
        [XmlAttribute]
        public int Index;

        [XmlAttribute]
        public float StartTime;

        [XmlAttribute]
        public bool IsJanitor;

        [XmlAttribute]
        public bool IsBook;

        [XmlAttribute]
        public string Text;
    }
}