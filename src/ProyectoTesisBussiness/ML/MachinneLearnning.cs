using ProyectoIA;
using ProyectoIA.TextClasification;
using ProyectoIA.Word2Vec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProyectoTesisBussiness.ML
{
    public class MachinneLearnning
    {
        MainClassLDA LDAClass = new MainClassLDA();
        MainWord2Vec word2Vec = new MainWord2Vec();
        MainTextClasification textClasification = new MainTextClasification();

        public (List<string>, List<string>) LDAAsync(List<string> trainSet)
        {
            var result = LDAClass.Entrenamiento(trainSet).Result;
            return result;
        }

        public string[] Tokens(string texto)
        {
            var listTokens = LDAClass.Tokens(texto);
            return listTokens?.Select(t => t.ToString())?.ToArray();
        }

        public List<string> WordToVec(string texto)
        {
            var vec = word2Vec.Entrenamiento(new List<string>() { texto }).Result;
            return vec;
        }

        public (List<string>, List<string>) TextClasification(List<string> trainSet)
        {
            var result = textClasification.Entrenamiento(trainSet).Result;
            return result;
        }
    }
}
