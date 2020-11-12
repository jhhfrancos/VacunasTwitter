using ProyectoLDA;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoTesisBussiness.ML
{
    public class MachinneLearnning
    {
        MainClassLDA LDAClass = new MainClassLDA();

        public List<Dictionary<string,string>> LDA(List<string> trainSet)
        {
            LDAClass.Entrenamiento(trainSet);
            return null;
        }
    }
}
