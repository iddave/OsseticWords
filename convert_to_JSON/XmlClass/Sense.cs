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
        [XmlAttribute("n")]
        public string Number;

        /// <summary>
        /// если у значения несколько переводов то они будут представлять собой последовательность cit елементов 
        /// </summary>
        [XmlElement("cit", IsNullable = false)]
        public List<Cit> Cits;

        [XmlElement("gramGrp", IsNullable = false)]
        public GramGroup? GramInfoGroup;

        [XmlElement("gloss", IsNullable = false)]
        public string? Gloss;

        public bool ShouldSerializeId()
        {
            return !string.IsNullOrEmpty(Number);
        }

        public Sense(List<Cit> cits, GramGroup gramInfoGroup = null, string? gloss = null, string num = "1")
        {
            GramInfoGroup = gramInfoGroup;
            Number = num;
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
