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

var jsonParser = new JSONParser();
var wordInfoList = jsonParser.UpdateWordsInfo(TxtDictionaryParser.Parse());
var testSubList = wordInfoList.GetRange(0, 20);
string  bodyNameXML = "osetWordsBody.xml",
        testBodyNameXML = "testWordsBody.xml",
        output = "XML_OssetWords.xml",
        testOutput = "XML_mini_words_test.xml";

// ДЛЯ КОНВЕРТАЦИИ В XML
XmlMethods.CreateBodyXml(bodyNameXML, wordInfoList);
XmlMethods.MergeAndSaveXml(bodyNameXML, output); // соедииняет голову и тело

////ДЛЯ СЕРИАЛИЗАЦИИ В JSON
SerializeToJson(true);

//TEST
//XmlMethods.CreateBodyXml(testBodyNameXML, testSubList);
//XmlMethods.MergeAndSaveXml(testBodyNameXML, testOutput); // соедииняет голову и тело



void SerializeToJson(bool all = false, int num = 1)
{
    var fileName = all ? "JSON_OssetWords.json" : "OsetWords_from" + num.ToString() + ".json";
    var path = @"..\..\..\output\" + fileName;
    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        JsonSerializer.Serialize(fs, wordInfoList, options);
    }
}





