// See https://aka.ms/new-console-template for more information
using convert_to_JSON;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using (FileStream fs = new FileStream("test2Words1.json", FileMode.OpenOrCreate))
{
    var lines = GetLines(1);
    var wordInfoList = new List<WordInfo>();

    foreach (var line in lines)
        wordInfoList.Add(ParseLine.GetWordInfo(line));

    var options = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        WriteIndented = true
    };
    JsonSerializer.Serialize(fs, wordInfoList, options);
}

IEnumerable<string> GetLines(int fileNum)
{
    var path = @"..\..\..\DictTxt\2.txt";
    using (StreamReader reader = new StreamReader(path))
    {
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line.Replace('ё', 'ӕ');
        }
    }
}

