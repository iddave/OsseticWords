using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    // переименовать класс и задать xml название
    internal class body
    {
        // XmlElement представляет последовательность элементов массива без родительского тега
        [XmlElement,
         XmlArray("entry")]
        public Entry[] SingleEntry;
    }
}
