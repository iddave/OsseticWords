using convert_to_JSON.XmlClass;
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
        public List<string> OrthographicForms;

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

        //[XmlElement("gramGrp", IsNullable = false)] 
        //public GramGroup? GramInfoGroup;

        public bool ShouldSerializeType()
        {
            return !string.IsNullOrEmpty(_type);
        }

        public EntryForm(List<string> orthographicForm, string? type = null) 
        {
            OrthographicForms = new List<string>(orthographicForm);
            Type = type;
        }

        public EntryForm(string orthographicForm, string? type = null)
        {
            OrthographicForms = new List<string>
            {
                orthographicForm
            };
            Type = type;
        }

        public EntryForm() { }

        public void AddOrthographicForm(string orth)
        {
            OrthographicForms.Add(orth);
        }
    }
}
