using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace convert_to_JSON
{
    public class UniparsedInfo
    {
        [JsonPropertyName("Лемма")]
        public string? Lemma { get; set; }

        [JsonPropertyName("Граммматтические теги")]
        public string? Gramm { get; set; }

        [JsonPropertyName("Gloss")]
        public string? Gloss { get; set; }

        [JsonPropertyName("Английский перевод")]
        public string? EnTranslation { get; set; }

        [JsonPropertyName("Русский перевод")]
        public string? RuTranslation { get; set; }

        public UniparsedInfo(string? lemma = null,
                            string? gramm = null,
                            string? gloss = null,
                            string? enTranslation = null,
                            string? ruTranslation = null) {
            Lemma= lemma;
            Gramm= gramm;
            Gloss= gloss;
            EnTranslation= enTranslation;
            RuTranslation= ruTranslation;
        }

        public UniparsedInfo() { }

        public override string? ToString()
        {
            return $"lemma: {Lemma}\n" +
                $"Gramm: {Gramm}\n" +
                $"Gloss: {Gloss}\n" +
                $"EnTranslation: {EnTranslation}\n" +
                $"RuTranslation: {RuTranslation}\n";
        }
    }
}
