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

                AddSenses(wordInfo.Meanings, entrySenses, entryID);
                AddAdditionalEntries(additionalEntries, wordInfo);

                entries.Add(new Entry(entryForm,
                                      entryID,
                                      entryTypeMain,
                                      entryLang,
                                      entrySenses,
                                      null,
                                      additionalEntries));
            }
        }

        public static void AddSenses(List<WordMeaning> wordMeanings, List<Sense> entrySenses, string entryID)
        {
            for (int i = 0; i < wordMeanings.Count(); i++)
            {
                var senseID = entryID + '.' + i;
                var senseTranlations = new List<Cit>();
                var senseExamples = new List<Cit>();

                AddTranslations(wordMeanings[i].Translations, senseTranlations);
                if (wordMeanings[i].Examples != null)
                    AddExamples(wordMeanings[i].Examples, senseExamples);

                ///переводы и примеры имеют один уровень иерархии и идут друг за другом. Сначала все переводы, потом все примеры.
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
    
        public static void AddAdditionalEntries(List<Entry> additionalEntries, WordInfo wordInfo)
        {
            var entryID = "UNIP."+wordInfo.IronWord;
            var entryLang = "oss";
            var entryTypeRelated = "relatedEntry";

            var subentryInd = 1;
            foreach (var token in wordInfo.UniparsedTokens)
            {
                var newEntryID = entryID + '.' + subentryInd;
                var entryForm = new EntryForm(token.Lemma, "lemma");
                var entrySense = new List<Sense>();
                var gramGroup = GetGramGroupForUniparsed(token);

                AddSenseForUniparsed(entrySense, token);

                additionalEntries.Add(new Entry(entryForm,
                                                newEntryID,
                                                entryTypeRelated,
                                                entryLang,
                                                entrySense,
                                                gramGroup));
                subentryInd++;
            }
        }

        public static void AddSenseForUniparsed(List<Sense> entrySense, UniparsedInfo uniparsedInfo)
        {
            var translationEn = new Cit();
            var translationRu = new Cit();
            var translations = GetTranslationsForUniparsed(uniparsedInfo);

            entrySense.Add(new Sense(translations, uniparsedInfo.Gloss));
        }

        public static List<Cit>? GetTranslationsForUniparsed(UniparsedInfo uniparsedInfo)
        {
            var translations = new List<Cit>();
            var enWord = uniparsedInfo.EnTranslation;
            var ruWord = uniparsedInfo.RuTranslation;
            if (enWord != null)
                translations.Add(new Cit("translationEquivalent", new EntryForm(enWord), "en"));
            if (ruWord != null)
                translations.Add(new Cit("translationEquivalent", new EntryForm(ruWord), "ru"));
            return translations.Count() == 0 ? null : translations;
        }

        public static GramGroup GetGramGroupForUniparsed(UniparsedInfo uniparsedInfo)
        {
            var jsonParser = new JSONParser();
            var gramTagsDictionary = jsonParser.GetGramTagsType();
            var gramGroup = new GramGroup();
            foreach(var gramTag in uniparsedInfo.Gramm.Split(','))
            {
                var gram = new Gram(gramTagsDictionary[gramTag], gramTag);
                gramGroup.AddGram(gram);
            }
            return gramGroup;
        }
    }
}
