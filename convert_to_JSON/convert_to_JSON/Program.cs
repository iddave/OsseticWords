// See https://aka.ms/new-console-template for more information
using convert_to_JSON;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Xml;
using System.Xml.Serialization;
using xmlClassesForLexicon;

const int FilesCount = 24;
bool all = true;
var lines = GetLines(all); // из всех файлов
var wordInfoList = new List<WordInfo>();
var uniqueWords = new Dictionary<string, int>();

foreach (var line in lines)
{
    wordInfoList.Add(ParseLine.GetWordInfo(line));
    var w = wordInfoList.Last().IronWord;
    if (!uniqueWords.ContainsKey(w))
    {
        uniqueWords[w] = 1;
    }
    else
    {
        var newW = w + '-' + (uniqueWords[w] + 1); // чтобы id были уникальные
        uniqueWords[newW] = 0;
        uniqueWords[w]++;
        wordInfoList.Last().IronWord = newW;
    }
}

//// для конвертации в xml
//XmlMethods.CreateBodyXml("osetExamples.xml", wordInfoList);
//XmlMethods.MergeAndSaveXml(); // соедииняет голову и тело

// для сериализации в json
SerializeToJson(true);

IEnumerable<string> GetLines(bool all = false, int fileNum = 1)
{

    if (all)
        for (int i = 1; i <= FilesCount; i++)
           foreach(string s in GetLinesFromOne(i)) yield return s;
    else foreach (string s in GetLinesFromOne(fileNum)) yield return s;
}

IEnumerable<string> GetLinesFromOne(int fileNum = 1)
{
    var path = @"..\..\..\DictTxt\" + fileNum.ToString() + ".txt"; // сохраняет в bin 
    using (StreamReader reader = new StreamReader(path))
    {
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line.Replace('ё', 'ӕ'); // в русских тоже меняет
        }
    }
}

void SerializeToJson(bool all = false, int num = 1)
{
    var fileName = all ? "AllOsetWords.json" : "OsetWords_from" + num.ToString() + ".json";
    using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        JsonSerializer.Serialize(fs, wordInfoList, options);
    }
}





