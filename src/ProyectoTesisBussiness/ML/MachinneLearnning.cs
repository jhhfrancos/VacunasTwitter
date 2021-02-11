using ProyectoIA;
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
        MainWord2Vec word2Vec = new MainWord2Vec();
        MainTextClasification textClasification = new MainTextClasification();

        public (List<string>, List<string>) LDAAsync(List<string> trainSet)
        {
            var trainReady = LDAClass.Training(trainSet).Result;
            var testingSet = new List<string>() { 
                "empiezan a sonar mucho las vacunas.Compensa dejar claro que las primeras que se usen habrán demostrado dos"
                ,"avanza la jornada departamental de vacunación las niñas ente 9 y 17 años recibirán la vacuna contra el VPH. Comunícate con tu Empresa Aseguradora de Planes de Beneficio y pide una cita, o el 29 de agosto acércate a la IPS vacunadora más cercana a tu casa" 
                ,"otro mito. Las vacunas son inseguras. FALSO Las vacunas pasan por una serie de análisis muy rígidos, que evalúan, primero"
                ,"Una vacuna hecha en tiempo récord cuando se necesitan 10 años para probar su efectividad y los efectos secundarios a largo plazo\n\n¿Podría causar problemas incluso generacionales? No se sabe, son muchas la dudas y pocas las garantías Vayan pasando, yo esperaré"
                ,"Aquí aclaramos las dudas que tienes sobre la vacuna del COVID-19"
                ,"Evidencias recientes refuerzan la elevada de las #vacunas. Las vacunas en uso tienen un excelente perfil de seguridad y proporcionan protección, individual y colectiva, frente a numerosas enfermedades infecciosas"
            };
            return LDAClass.Testing(testingSet).Result; ;
        }

        public string[] Tokens(string texto)
        {
            var listTokens = LDAClass.Tokens(texto);
            return listTokens?.Select(t => t.ToString())?.ToArray();
        }

        public IEnumerable<FrequencyWord> WordToVec(List<string> texto)
        {
            var vec = word2Vec.Trainning(texto).Result;
            return vec;
        }

        public (List<string>, List<string>) TextClasification(List<string> trainSet)
        {
            var result = textClasification.Entrenamiento(trainSet).Result;
            return result;
        }
    }
}
