using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    // переименовать класс и задать xml название
    [XmlRoot("text")]
    public class Text
    {
        // XmlElement представляет последовательность элементов массива без родительского тега
        //[XmlElement,
        // XmlArray("entry")]
        [XmlArray("body", IsNullable = false),
         XmlArrayItem("entry")]
        public List<Entry>? SingleEntry;

        public Text()
        {
            SingleEntry = new List<Entry>();
        }
        public Text(List<Entry> entries)
        {
            SingleEntry = new List<Entry>(entries);
        }
    }
}
