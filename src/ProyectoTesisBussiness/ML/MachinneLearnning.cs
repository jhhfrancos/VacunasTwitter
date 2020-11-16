using ProyectoLDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProyectoTesisBussiness.ML
{
    public class MachinneLearnning
    {
        MainClassLDA LDAClass = new MainClassLDA();

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
    }
}
