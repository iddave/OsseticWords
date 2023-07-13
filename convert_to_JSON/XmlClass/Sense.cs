using convert_to_JSON.XmlClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    [XmlRoot("sense")]
    public class Sense
    {
        private string _id;
        [XmlAttribute("xml:id")] 
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// если у значения несколько переводов то они будут представлять собой последовательность cit елементов 
        /// </summary>
        [XmlElement("cit", IsNullable = false)]
        public List<Cit> Cits;

        [XmlElement("gloss", IsNullable = false)]
        public string? Gloss;

        public bool ShouldSerializeId()
        {
            return !string.IsNullOrEmpty(Id);
        }

        public Sense(string id, List<Cit> cits, string? gloss = null)
        {
            Id = id;
            Cits = cits;
            Gloss = gloss;
        }

        public Sense(List<Cit> cits, string? gloss = null)
        {
            Cits = cits;
            Gloss = gloss;
        }

        public Sense() { }
    }
}
