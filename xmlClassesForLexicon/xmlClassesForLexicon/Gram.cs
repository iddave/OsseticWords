using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    internal class Gram
    {
        [XmlAttribute,
         XmlElement(ElementName = "type")]
        public string Type;
        [XmlAttribute,
         XmlElement(ElementName = "value")]
        public string Value;
    }
}
