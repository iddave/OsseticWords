using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace convert_to_JSON
{
    public class ParseLine
    {
        public static WordInfo GetWordInfo(string WordStr)
        {
            var wMeanings = new List<WordMeaning>();
            var ar = WordStr.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var wInfo = new WordInfo(ar[0]);
            CheckEvenAndSymb(WordStr);
            if (ar.Count() < 3) throw new ArgumentException($"В строке {WordStr} чего-то не хватает");
            var seps = new char[] { '$', '#' };
            var means = ar[2].Split(seps, StringSplitOptions.RemoveEmptyEntries);
            foreach (var m in means)
            {
                var meaningStr = m.Trim();
                CheckAdditionalWord(ref wInfo, ref meaningStr);
                //Console.WriteLine($"осет - {wInfo.IronW}, доп слово - {wInfo.AdditionalIronW}, остаток строки - {meaningStr} ");
                var meaning = ParseMeaning(meaningStr);
                wMeanings.Add(meaning);
            }
            wInfo.Meanings = wMeanings;
            return wInfo;
            //wInfo.Print();
            //var options = new JsonSerializerOptions { WriteIndented = true, AllowTrailingCommas = true };
            //JsonSerializer.Serialize(fs, wInfo, options);
        }

        static void CheckAdditionalWord(ref WordInfo w, ref string s)
        {
            if (s.StartsWith('&') || s.StartsWith("мн.ч") || s.StartsWith("см."))
            {
                string addWord;
                var ind = s.IndexOf('&', s.IndexOf('&') + 1);
                try
                {
                    addWord = s.Substring(0, ind);
                }
                catch(ArgumentOutOfRangeException e)
                {
                    throw new ArgumentOutOfRangeException(s);
                }
                w.AdditionalIronW = addWord.Replace("&", "");
                s = s.Substring(ind + 1).Trim();
            }
        }

        static WordMeaning ParseMeaning(string s)
        {
            var endSeps = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ',', ';', '$', '#', ' ' };
            var ind = s.IndexOf('&');
            var rusWordsList = new List<string>();
            var examplesList = new List<Tuple<string, string>>();
            if (ind != -1)
            {
                var rusWords = s.Substring(0, ind).Trim(endSeps);
                var examples = s.Substring(ind).Trim(endSeps);
                //Console.WriteLine($"rus words: {rusWords} examples: {examples}");
                try
                {
                    rusWordsList = ParseRusWords(rusWords);
                    examplesList = ParseExamples(examples);
                }
                catch (Exception e)
                {
                    rusWordsList.Add(rusWords);
                    examplesList.Add(Tuple.Create("? ", examples));
                    Console.WriteLine($"Ошибка - {e} в слове - {s}");
                }
            }
            else
            {
                rusWordsList = ParseRusWords(s.Trim(endSeps));
                //Console.WriteLine(s);
            }

            return new WordMeaning(rusWordsList, examplesList);
        }

        static List<string> ParseRusWords(string s)
        {
            var seps = new char[] { ',', ';' };
            return s.Split(seps, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList(); //сплитит лишние запятые, нужно исправлять руками
        }

        static List<Tuple<string, string>> ParseExamples(string s)
        {
            var result = new List<Tuple<string, string>>();
            var seps = new char[] { ',', ';', ' ' };
            var ar = s.Split('&', StringSplitOptions.RemoveEmptyEntries);

            if (s.StartsWith('&') && s.EndsWith('&')) result.Add(Tuple.Create("ссылка на слово",s));
            else if (ar.Count() % 2 != 0) throw new ArgumentException($"что-то не так в примере употреблений: {s}");
            
            for (int i = 0; i < ar.Count(); i += 2)
                result.Add(Tuple.Create(ar[i].Trim(seps), ar[i + 1].Trim(seps)));
            
            return result;
        }

        static void CheckEvenAndSymb(string s)
        {
            var count = s.Where(z => z == '&').Count();
            if (count % 2 != 0) throw new ArgumentException($"Нечетное количество & в слове {s}");
        }
    }
}
