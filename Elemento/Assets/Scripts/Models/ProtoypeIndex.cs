using System;
using System.Xml.Serialization;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ProtoypeIndex
    {
        [XmlAttribute]
        public string Uri;

        [XmlIgnore]
        public string ProtoypeType
        {
            get { return !string.IsNullOrEmpty(Uri) ? Uri.Split(':')[0] : "unset"; }
        }

        [XmlAttribute]
        public string Path;
    }
}