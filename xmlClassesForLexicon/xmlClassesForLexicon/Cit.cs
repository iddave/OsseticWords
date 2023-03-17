using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    internal class Cit
    {
        /// <summary>
        /// значения:
        /// translationEquivalent - перевод для слов
        /// example - для примера
        /// translation - для перевода примера
        /// </summary>
        [XmlAttribute,
         XmlElement(ElementName = "type")]
        public string Translation;

        [XmlAttribute,
         XmlElement(ElementName = "xml:lang"),
         XmlElement(IsNullable = false)]
        public string Lang;

        [XmlElement(ElementName = "form")]
        public Form TranslForm;

        [XmlElement(ElementName = "quote"),
         XmlElement(IsNullable = false)]
        public string Quote;

        [XmlElement(ElementName = "cit"),
         XmlElement(IsNullable = false)]
        public Cit NestedCit;

    }
}
