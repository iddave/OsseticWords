using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    [XmlRoot("gram")]
    public class Gram
    {
        [XmlAttribute("type")]
        public string Type;

        [XmlAttribute("value")]
        public string Value;

        public Gram(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public Gram() { }
    }
}
