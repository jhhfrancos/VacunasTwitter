﻿using Catalyst;
using Catalyst.Models;
using Mosaik.Core;
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

        public async Task<List<string>> Entrenamiento(List<string> stringArray)
        {
            
            List<IDocument> training = new List<IDocument>();
            foreach (var item in stringArray)
            {
                training.Add(new Document(item, Language.Spanish));
            }

            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models-word2vec"));

            var nlp = await Pipeline.ForAsync(Language.Spanish);

            var ft = new FastText(Language.Spanish, 0, "wiki-word2vec");
            ft.Data.Type = FastText.ModelType.CBow;
            ft.Data.Loss = FastText.LossType.NegativeSampling;
            ft.Train(nlp.Process(training));
            //await ft.StoreAsync();

            
            return training.Select(t => t.ToJson()).ToList();
        }

    }
}