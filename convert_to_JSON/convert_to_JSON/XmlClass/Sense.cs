using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    [XmlRoot("sense")]
    public class Sense
    {
        [XmlAttribute("xml:id")] 
        public string Id;

        /// <summary>
        /// если у значения несколько переводов то они будут представлять собой последовательность cit елементов 
        /// </summary>
        [XmlElement("cit", IsNullable = false)]
        public List<Cit> Cits;

        [XmlElement("gramGrp", IsNullable = false)] // будет ли отображаться корректно без данных gram
        public List<Gram>? GramInfoGroup;

        public Sense(string id, List<Cit> cits, List<Gram>? gramInfoGroup = null)
        {
            Id = id;
            Cits = cits;
            GramInfoGroup = gramInfoGroup;
        }

        public Sense() { }
    }
}
