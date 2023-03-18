using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xmlClassesForLexicon
{
    [XmlRoot("cit")]
    public class Cit
    {
        private string? _type;
        private string? _lang;
        /// <summary>
        /// значения:
        /// translationEquivalent - перевод для слов
        /// example - для примера
        /// translation - для перевода примера
        /// </summary>
        [XmlAttribute("type")] 
        public string? Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [XmlAttribute("xml:lang")] 
        public string? Lang
        {
            get { return _lang; }
            set { _lang = value; }
        }

        [XmlElement("form", IsNullable = false)]
        public EntryForm? TranslForm;

        [XmlElement("quote", IsNullable = false)]
        public string? Quote;

        [XmlElement("cit", IsNullable = false)]
        public Cit? NestedCit;

        /// <summary>
        /// методы вызываются при сериализации для проверки null у xml атрибутов 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeType()
        {
            return !string.IsNullOrEmpty(_type);
        }

        public bool ShouldSerializeLang()
        {
            return !string.IsNullOrEmpty(_lang);
        }

        public Cit(string type, EntryForm form, string lang)
        {
            Type = type;
            Lang = lang;
            TranslForm = form;
        }

        public Cit(string type, string quote, string lang)
        {
            Type = type;
            Lang = lang;
            Quote = quote;
        }

        public Cit(string type, string quote, Cit? nestedCit)
        {
            Quote = quote;
            NestedCit = nestedCit; // ???
            Type = type;
        }

        public Cit() { }
    }
}
