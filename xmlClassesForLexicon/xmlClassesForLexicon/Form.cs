using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    internal class Form
    {
        [XmlElement(ElementName = "orth")]
        public string OrthographicForm;

        [XmlElement(ElementName = "type")]
        public string Type;

        [XmlArray("gramGrp"),
         XmlArrayItem("gram"),
         XmlElement(IsNullable = false)] // будет ли отображаться корректно без данных gram
        public Gram[] GramInfoGroup;


    }
}
