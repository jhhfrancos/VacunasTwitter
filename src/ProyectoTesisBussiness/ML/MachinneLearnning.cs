﻿using Catalyst;
using ProyectoIA;
using ProyectoIA.NER;
using ProyectoIA.SpanishStemmer;
using ProyectoIA.TextClasification;
using ProyectoIA.Word2Vec;
using ProyectoTesisModels.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProyectoTesisBussiness.ML
{
    public class MachinneLearnning
    {
        MainClassLDA LDAClass = new MainClassLDA();
        MainClassNER NERClass = new MainClassNER();
        MainWord2Vec word2Vec = new MainWord2Vec();
        MainTextClasification textClasification = new MainTextClasification();
        Stemmer stemming = new Stemmer();

        public bool LDATrainAsync(List<string> trainSet, int numTopics, int NumOfTerms)
        {
            var trainReady = LDAClass.Training(trainSet, numTopics, NumOfTerms).Result;
            
            return trainReady;
        }

        public (List<string>, List<string>) LDATestResultAsync(List<string> testingSet)
        {
            return LDAClass.Testing(testingSet).Result;
        }

        public string[] Tokens(string texto)
        {
            var listTokens = LDAClass.Tokens(texto);
            return listTokens?.Select(t => t.ToString())?.ToArray();
        }

        public IEnumerable<FrequencyWord> WordToVec(List<string> texto)
        {
            var vec = word2Vec.Trainning(texto).Result;
            return vec.GroupBy(t => stemming.Execute(t.Token), StringComparer.OrdinalIgnoreCase)
                .Select(t => new FrequencyWord() { word = t.Key, size = t.First().Frequency })
                .Where(t => t.word != "</s>" && t.word.Length > 2)
                .Skip((texto.Count * 10) / 100)
                .Take((texto.Count * 80) / 100);
        }

        public bool NER(List<string> texto)
        {
            _ = NERClass.Trainning(texto);
            return true;
        }

        public TableTopics NERTest(string texto)
        {
            var NERResult = NERClass.Testing(texto).Result.ToArray();
            return new TableTopics(texto, "", NERResult);
        }
        public (List<string>, List<string>) LDATest(string text)
        {
            return LDAClass.Testing(new List<string>() { text }).Result; ;
        }


        public (List<string>, List<string>) TextClasification(List<string> trainSet)
        {
            var result = textClasification.Entrenamiento(trainSet).Result;
            return result;
        }
    }
}
