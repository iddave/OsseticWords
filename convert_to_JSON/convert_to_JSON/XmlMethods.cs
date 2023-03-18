using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using xmlClassesForLexicon;

namespace convert_to_JSON
{
    public class XmlMethods
    {
        public static void MergeAndSaveXml()
        {
            XmlDocument header = new XmlDocument(),
                        body = new XmlDocument();
            header.Load(@"..\..\..\DictTxt\OssHeader.xml");
            body.Load("osetExamples.xml");

            XmlNode root1 = header.DocumentElement,
                    root2 = body.DocumentElement;

            XmlDocument mergedDoc = new XmlDocument();
            XmlElement newRoot = mergedDoc.CreateElement("TEI");
            XmlAttribute xmlnsAttribute = mergedDoc.CreateAttribute("xmlns");
            xmlnsAttribute.Value = @"http://www.tei-c.org/ns/1.0";
            newRoot.Attributes.Append(xmlnsAttribute);
            mergedDoc.AppendChild(newRoot);

            XmlNode importedRoot1 = mergedDoc.ImportNode(root1, true);
            newRoot.AppendChild(importedRoot1);
            XmlNode importedRoot2 = mergedDoc.ImportNode(root2, true);
            newRoot.AppendChild(importedRoot2);

            XmlProcessingInstruction validationString = mergedDoc.CreateProcessingInstruction("xml-model", "href=\"https://raw.githubusercontent.com/DARIAH-ERIC/lexicalresources/master/Schemas/TEILex0/out/TEILex0.rng\" type=\"application/xml\" schematypens=\"http://relaxng.org/ns/structure/1.0\"");
            mergedDoc.InsertBefore(validationString, mergedDoc.DocumentElement);

            mergedDoc.Save("OsetExamples.xml");
        }

        public static void CreateBodyXml(string filename, List<WordInfo> wordInfoList)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Text));
            TextWriter writer = new StreamWriter(filename);
            var entries = new List<Entry>();

            AddEntries(wordInfoList, entries);
            Text xmlBody = new Text(entries);
            serializer.Serialize(writer, xmlBody);
            writer.Close();
        }

        public static void AddEntries(List<WordInfo> wordInfoList, List<Entry> entries)
        {
            foreach (var word in wordInfoList)
            {
                var entryID = word.IronWord;
                var entryLang = "oss";
                var entryForm = new EntryForm(word.IronWord, "lemma");
                var entrySenses = new List<Sense>();

                AddSenses(word.Meanings, entrySenses, entryID);

                entries.Add(new Entry(entryForm, entryID, entryLang, entrySenses));
            }
        }

        public static void AddSenses(List<WordMeaning> wordMeanings, List<Sense> entrySenses, string entryID)
        {
            for (int i = 0; i < wordMeanings.Count(); i++)
            {
                var senseID = entryID + '.' + i;
                var senseTranlations = new List<Cit>();
                var senseExamples = new List<Cit>();

                AddTranslations(wordMeanings[i].RusWord, senseTranlations);
                if (wordMeanings[i].Examples != null)
                    AddExamples(wordMeanings[i].Examples, senseExamples);

                senseTranlations.AddRange(senseExamples);
                entrySenses.Add(new Sense(senseID, senseTranlations));
            }
        }

        public static void AddTranslations(List<string> rusWords, List<Cit> senseTranlations)
        {
            foreach (var translation in rusWords)
            {
                var translForm = new EntryForm(translation);
                var translCit = new Cit("translationEquivalent", translForm, "ru");

                senseTranlations.Add(translCit);
            }
        }

        public static void AddExamples(List<Tuple<string, string>> examples, List<Cit> senseExamples)
        {
            foreach (var example in examples)
            {
                var exmpFrom = example.Item1;
                var exmpTo = example.Item2;
                var translExmpCit = new Cit("translation", exmpTo, "ru");
                var exmpCit = new Cit("example", exmpFrom, translExmpCit);

                senseExamples.Add(exmpCit);
            }
        }
    }
}
