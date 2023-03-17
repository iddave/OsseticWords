using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    internal class Sense
    {
        [XmlAttribute,
         XmlElement(ElementName = "xml:id")] 
        public string Id;

        /// <summary>
        /// если у значения несколько переводов то они будут представлять собой последовательность cit елементов 
        /// </summary>
        [XmlElement,
         XmlArray("cit"),
         XmlElement(IsNullable = false)]
        public Cit[] Cits;

        [XmlArray("gramGrp"),
         XmlArrayItem("gram"),
         XmlElement(IsNullable = false)] // будет ли отображаться корректно без данных gram
        public Gram[] GramInfoGroup;

    }
}
