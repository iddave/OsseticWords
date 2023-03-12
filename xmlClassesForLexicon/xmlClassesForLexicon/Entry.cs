using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    internal class Entry
    {
        [XmlElement(ElementName = "form")]
        public Form FormInformationGroup;

        [XmlAttribute,
         XmlElement(ElementName = "xml:id")] // возможно неправильно задал имя атрибуту
        public string Id;
        [XmlAttribute,
         XmlElement(ElementName = "xml:lang")]
        public string Lang;
    }
}
