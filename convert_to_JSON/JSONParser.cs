using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace convert_to_JSON
{
    public class JSONParser
    {
        public string uniparsedFilePath = @"..\..\..\source files\uniparsed_osseticWords.json";
        public string gramTagsFilePath = @"..\..\..\source files\gram_tags.json";

        public JSONParser() { }

        public List<WordInfo> UpdateWordsInfo(List<WordInfo> wordInfoList)
        {

            // Read the entire JSON file as a string
            string jsonString = File.ReadAllText(uniparsedFilePath);

            // Parse the JSON string into a JsonDocument
            JsonDocument jsonDocument = JsonDocument.Parse(jsonString);

            // Access the root element of the JSON document
            JsonElement rootElement = jsonDocument.RootElement;

            // Deserialize the root element as a dictionary
            Dictionary<string, JsonElement> jsonDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(rootElement.GetRawText());

            foreach(var wordInfo in wordInfoList)
            {
                var allUniparsedInfo = new List<UniparsedInfo>();
                JsonElement analyzeVariations = new JsonElement();
                try
                {
                    analyzeVariations = jsonDictionary[CheckPattern(wordInfo.IronWord)];
                }
                catch(KeyNotFoundException ex)
                {
                    continue;
                }

                foreach(JsonElement analyzeVariation in analyzeVariations.EnumerateArray())
                {
                    allUniparsedInfo.Add(GetUniparsedInfo(analyzeVariation));
                }
                wordInfo.UniparsedTokens = allUniparsedInfo;
            }
            return wordInfoList;
        }

        public UniparsedInfo GetUniparsedInfo(JsonElement jsonElement)
        {
            string lemma = jsonElement.GetProperty("lemma").GetString();
            string gramm = jsonElement.GetProperty("gramm").GetString();
            string gloss = jsonElement.GetProperty("gloss").GetString();
            List<string> translations = new List<string>();
            UniparsedInfo result = new UniparsedInfo();

            foreach (JsonElement subElement in jsonElement.GetProperty("otherData").EnumerateArray())
            {
                string key = subElement[0].GetString();
                string value = subElement[1].GetString();
                translations.Add(value);
            }
            try
            {
                result =  new UniparsedInfo(lemma, gramm, gloss, translations[0], translations[1]);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                result = new UniparsedInfo(lemma, gramm, gloss);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string CheckPattern(String word)
        {
            string pattern = @"^([^-]+)";

            Match match = Regex.Match(word, pattern); // для повторяющихся слов e.g. агъуыд-2

            if (match.Success)
            {
                string extractedString = match.Groups[1].Value;
                return extractedString; // Output: "агъуыд"
            }
            return word;
        }

        public Dictionary<string, string> GetGramTagsType()
        {
            string jsonString = File.ReadAllText(gramTagsFilePath);

            List<string[]> jsonArray = JsonSerializer.Deserialize<List<string[]>>(jsonString);

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string[] array in jsonArray)
            {
                string key = array[1];
                string value = array[0];
                dictionary[key] = value;
            }
            return dictionary;
        }
    }
}
