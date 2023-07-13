using convert_to_JSON.XmlClass;
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

        public static void MergeAndSaveXml(string bodyName, string output)
        {
            XmlDocument header = new XmlDocument(),
                        body = new XmlDocument();
            header.Load(@"..\..\..\source files\DictTxt\OssHeader.xml");
            body.Load(bodyName);

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


            mergedDoc.Save(@"..\..\..\output\"+output);
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
            foreach (var wordInfo in wordInfoList)
            {
                var entryID = wordInfo.IronWord;
                var entryLang = "oss";
                var entryTypeMain = "mainEntry";
                var entryForm = new EntryForm(wordInfo.IronWord, "lemma");
                var entrySenses = new List<Sense>();
                var additionalEntries = new List<Entry>();

                AddSenses(wordInfo, entrySenses);
                //AddAdditionalEntries(additionalEntries, wordInfo);

                entries.Add(new Entry(entryForm,
                                      entryID,
                                      entryTypeMain,
                                      entryLang,
                                      entrySenses));
            }
        }


        public static void AddSenses(WordInfo wordInfo, List<Sense> entrySenses)
        {
            var wordMeanings = wordInfo.Meanings;
            for (int i = 0; i < wordMeanings.Count(); i++)
            {
                
                var senseTranlations = new List<Cit>();
                var senseExamples = new List<Cit>();

                AddTranslations(wordMeanings[i].Translations, senseTranlations);
                if (wordMeanings[i].Examples != null)
                    AddExamples(wordMeanings[i].Examples, senseExamples);

                GramGroup gramGroup = null;
                if (wordInfo.UniparsedTokens.Count() == 1
                && wordMeanings.Count() == 1)
                {
                    gramGroup = GetGramGroupForUniparsed(wordInfo.UniparsedTokens[0]);
                    AddTranslationsFromUniparser(wordInfo.UniparsedTokens[0], senseTranlations);
                }

                ///переводы и примеры имеют один уровень иерархии и идут друг за другом. Сначала все переводы, потом все примеры.
                senseTranlations.AddRange(senseExamples); 
                entrySenses.Add(new Sense(senseTranlations, gramGroup, null, (i + 1).ToString()));
            }
        }

        public static void AddTranslations(List<string> rusWords, List<Cit> senseTranlations)
        {
            var translations = new List<string>();
            foreach (var translation in rusWords)
            {
                translations.Add(translation);
            }
            var translForm = new EntryForm(translations);
            var translCit = new Cit("translationEquivalent", translForm, "ru");
            senseTranlations.Add(translCit);
        }

        public static void AddTranslationsFromUniparser(UniparsedInfo token, List<Cit> senseTranlations)
        {
            if (token.EnTranslation != null && !string.IsNullOrEmpty(token.EnTranslation.Trim()))
            {
                var translForm = new EntryForm(token.EnTranslation);
                var translCit = new Cit("translationEquivalent", translForm, "en");
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

        public static GramGroup GetGramGroupForUniparsed(UniparsedInfo uniparsedInfo)
        {
            //var jsonParser = new JSONParser();
            //var gramTagsDictionary = jsonParser.GetGramTagsType();
            var gramGroup = new GramGroup();
            foreach(var gramTag in uniparsedInfo.Gramm.Split(','))
            {
                //var gram = new Gram(gramTagsDictionary[gramTag], gramTag);
                var gram = new Gram(gramTag);
                gramGroup.AddGram(gram);
            }
            return gramGroup;
        }
    }
}
