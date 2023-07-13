using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using xmlClassesForLexicon;

namespace convert_to_JSON.XmlClass
{
    [XmlRoot("gramGrp")]
    public class GramGroup
    {
        [XmlElement("gram")]
        public List<Gram> Grams;

        public GramGroup() {
            Grams = new List<Gram>();
        }

        public GramGroup(List<Gram> grams)
        {
            Grams = grams;
        }

        public void AddGram(Gram gram)
        {
            Grams.Add(gram);
        }
    }
}
