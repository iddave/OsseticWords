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

var lines = GetLines(1);
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

XmlMethods.CreateBodyXml("osetExamples.xml", wordInfoList);
XmlMethods.MergeAndSaveXml(); // соедииняет голову и тело
//SerializeToJson();

IEnumerable<string> GetLines(int fileNum)
{
    var path = @"..\..\..\DictTxt\2.txt";
    using (StreamReader reader = new StreamReader(path))
    {
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line.Replace('ё', 'ӕ'); // в русских тоже меняет
        }
    }
}

void SerializeToJson()
{
    using (FileStream fs = new FileStream("test2Words1.json", FileMode.OpenOrCreate))
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        JsonSerializer.Serialize(fs, wordInfoList, options);
    }
}





