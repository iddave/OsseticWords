using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    [XmlRoot("entry")]
    public class EntryForm
    {
        private string? _type;

        [XmlElement(ElementName = "orth")]
        public string OrthographicForm;

        /// <summary>
        /// Несколько примеров значений:
        /// lemma - the headword itself
        /// variant - a variant form                
        /// </summary>
        [XmlAttribute("type")]
        public string? Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [XmlElement("gramGrp", IsNullable = false)] // будет ли отображаться корректно без данных gram
        public List<Gram>? GramInfoGroup;

        public bool ShouldSerializeType()
        {
            return !string.IsNullOrEmpty(_type);
        }

        public EntryForm(string orthographicForm, string? type = null) 
        {
            OrthographicForm = orthographicForm;
            Type = type;
        }

        public EntryForm() { }
    }
}
