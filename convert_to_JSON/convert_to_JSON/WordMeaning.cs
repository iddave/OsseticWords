﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace convert_to_JSON
{
    public class WordMeaning
    {
        [JsonPropertyName("Переводы")]
        public List<string> RusWord { get; set; }

        [JsonPropertyName("Примеры")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Tuple<string, string>>? Examples { get; set; }
        //public Tuple<string, string> t;

        public WordMeaning()
        {
            RusWord = new List<string>();
            Examples = new List<Tuple<string, string>>();
            //t = new Tuple<string, string>("str1", "str2");
        }

        public WordMeaning(List<string> rusW, List<Tuple<string, string>> examples)//?
        {
            RusWord = rusW;
            Examples = examples.Count() == 0 ? null : examples;
        }

        public void Print()
        {
            Console.Write($"Meanings: ");
            foreach (var w in RusWord)
                Console.Write(w + ' ');
            Console.Write($"\nExamples: ");
            if(Examples!=null)
                foreach (var e in Examples)
                    Console.WriteLine($"   {e.Item1} - {e.Item2}");
            Console.WriteLine();
        }
    }
}
