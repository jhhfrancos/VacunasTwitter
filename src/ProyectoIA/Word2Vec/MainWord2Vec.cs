using Catalyst;
using Catalyst.Models;
using Mosaik.Core;
using ProyectoTesisModels.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoIA.Word2Vec
{
    public class MainWord2Vec
    {
        public MainWord2Vec()
        {

        }

        public async Task<IEnumerable<FrequencyWord>> Trainning(List<string> stringArray)
        {
            
            List<IDocument> training = new List<IDocument>();
            foreach (var item in stringArray)
            {
                //var newItem = Utils.Utils.DeleteStopWords(item);
                training.Add(new Document(item, Language.Spanish));
            }

            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models-word2vec"));

            var nlp = await Pipeline.ForAsync(Language.Spanish);

            var ft = new FastText(Language.Spanish, 0, "wiki-word2vec");
            ft.Data.Type = FastText.ModelType.CBow;
            ft.Data.Loss = FastText.LossType.NegativeSampling;
            //ft.Data.ContextWindow = 2;
            ft.Data.IgnoreCase = true;

            ft.Train(nlp.Process(training));
            var vectors = ft.GetVectors();
            //await ft.StoreAsync();
            
            return 
                vectors
                .GroupBy(t => t.Token, StringComparer.OrdinalIgnoreCase)
                .Select(t => new FrequencyWord() { word = t.Key, size = t.First().Frequency });
        }


    }
}
