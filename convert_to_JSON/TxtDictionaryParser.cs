using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace convert_to_JSON
{
    public class TxtDictionaryParser
    {
        public const int FilesCount = 24;
        public static bool FromAllFiles = true;
        public const string SourcePath = @"..\..\..\source files\DictTxt\";

        public static List<WordInfo> Parse()
        {
            var lines = GetLines(FromAllFiles);
            var wordInfoList = new List<WordInfo>();
            var uniqueWords = new Dictionary<string, int>();

            foreach (var line in lines)
            {
                wordInfoList.Add(GetWordInfo(line));
                var currentIronWord = wordInfoList.Last().IronWord;
                if (!uniqueWords.ContainsKey(currentIronWord))
                {
                    uniqueWords[currentIronWord] = 1;
                }
                else
                {
                    var modifiedIronWord = currentIronWord + '-' + (uniqueWords[currentIronWord] + 1); // чтобы id были уникальные
                    uniqueWords[currentIronWord]++;
                    wordInfoList.Last().IronWord = modifiedIronWord;
                }
            }

            return wordInfoList;
        }
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
                var meaning = ParseMeaning(meaningStr);
                wMeanings.Add(meaning);
            }
            wInfo.Meanings = wMeanings;
            return wInfo;
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
                //Console.WriteLine(exampleString);
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

        static List<Tuple<string, string>> ParseExamples(string exampleString)
        {
            var result = new List<Tuple<string, string>>();
            var seps = new char[] { ',', ';', ' ' };
            var examplesArr = exampleString.Split('&', StringSplitOptions.RemoveEmptyEntries);

            if (exampleString.StartsWith('&') && exampleString.EndsWith('&')) result.Add(Tuple.Create("ссылка на слово",exampleString));
            else if (examplesArr.Count() % 2 != 0) throw new ArgumentException($"что-то не так в примере употреблений: {exampleString}");
            try
            {
                for (int i = 0; i < examplesArr.Count(); i += 2)
                    result.Add(Tuple.Create(examplesArr[i].Trim(seps), examplesArr[i + 1].Trim(seps)));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ошибка в примерах {exampleString}");
            }
            return result;
        }

        static void CheckEvenAndSymb(string s)
        {
            var count = s.Where(z => z == '&').Count();
            if (count % 2 != 0) throw new ArgumentException($"Нечетное количество & в слове {s}");
        }

        static IEnumerable<string> GetLines(bool all = false, int fileNum = 1)
        {

            if (all)
                for (int i = 1; i <= FilesCount; i++)
                    foreach (string s in GetLinesFromOne(i)) yield return s;
            else foreach (string s in GetLinesFromOne(fileNum)) yield return s;
        }

        static IEnumerable<string> GetLinesFromOne(int fileNum = 1)
        {
            var path = SourcePath + fileNum.ToString() + ".txt"; // сохраняет в bin 
            using (StreamReader reader = new StreamReader(path))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line.Replace('ё', 'ӕ'); // в русских тоже меняет
                }
            }
        }
    }
}
