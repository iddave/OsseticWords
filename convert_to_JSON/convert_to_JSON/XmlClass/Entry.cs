using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    [XmlRoot("entry")]
    public class Entry
    {
        [XmlElement("form", IsNullable = false)]
        public EntryForm FormInformationGroup;

        [XmlAttribute("xml:id")]
        public string Id;

        [XmlAttribute("xml:lang")]
        public string Lang;

        [XmlElement("sense")] // null?
        public List<Sense> Senses;

        public Entry(EntryForm formInformationGroup, string id, string lang, List<Sense> senses)
        {
            FormInformationGroup = formInformationGroup;
            Id = id;
            Lang = lang;
            Senses = senses;
        }

        public Entry() { }
    }
}
