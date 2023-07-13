using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace convert_to_JSON
{
    public class WordInfo
    {
        [JsonPropertyName("Осетинское слово")]
        public string IronWord { get; set; }

        [JsonPropertyName("Доп. слово")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AdditionalIronW { get; set; }

        [JsonPropertyName("Значения")]
        public List<WordMeaning> Meanings { get; set; }

        [JsonPropertyName("Uniparsed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<UniparsedInfo> UniparsedTokens { get; set; }

        public WordInfo(string word = "none")//?
        {
            IronWord = word;
            AdditionalIronW = null;
            Meanings = new List<WordMeaning>();
            UniparsedTokens = new List<UniparsedInfo>();
        }

        public override string? ToString()
        {
            return $"Iron: {IronWord}\n" +
                $"Additional: {AdditionalIronW}\n" +
                $"Meanings: {string.Join("  \n", Meanings.Select(obj => obj.ToString()))}\n" +
                $"Uniparsed: {string.Join("  \n", UniparsedTokens.Select(obj => obj.ToString()))}\n";
        }

        public void Print()
        {
            Console.WriteLine($"Iron: {this.IronWord}\nAdditional: {this.AdditionalIronW}");
            foreach (var m in Meanings)
                m.Print();
            Console.WriteLine();
        }
    }
}
