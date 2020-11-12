using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalyst;
using Catalyst.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Mosaik.Core;

namespace ProyectoLDA
{
    public class MainClassLDA
    {
        public async void Entrenamiento(List<string> stringArray)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ApplicationLogging.SetLoggerFactory(LoggerFactory.Create(lb => lb.AddConsole()));

            //Configures the model storage to use the online repository backed by the local folder ./catalyst-models/
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models-viajes"));

            //Download the Reuters corpus if necessary
            List<IDocument> training = new List<IDocument>();
            foreach (var item in stringArray)
            {
                training.Add(new Document(item, Language.Spanish));
            }

            IDocument[] train = training.ToArray();


            ///////////-------------
            List<IDocument> testing = new List<IDocument>();
            testing.Add(new Document("Me gusta esta de viaje entre carreteras y amigos", Language.Spanish));
            testing.Add(new Document("Salir de vaciones", Language.Spanish));
            testing.Add(new Document("ciudad allá voy", Language.Spanish));
            testing.Add(new Document("todas las fotografias del mundo junto a la gente que amo", Language.Spanish));

            IDocument[] test = testing.ToArray();
            //var (train, test) = await Corpus.Reuters.GetAsync();

            //Parse the documents using the English pipeline, as the text data is untokenized so far
            var nlp = Pipeline.For(Language.Spanish);

            var trainDocs = nlp.Process(train).ToArray();
            
            ///////////////////----------------
            var testDocs = nlp.Process(test).ToArray();


            //Train an LDA topic model on the trainind dateset
            using (var lda = new LDA(Language.Spanish, 0, "viajes-lda"))
            {
                lda.Data.NumberOfTopics = 20; //Arbitrary number of topics
                lda.Train(trainDocs, Environment.ProcessorCount);
                await lda.StoreAsync();
            }

            using (var lda = await LDA.FromStoreAsync(Language.Spanish, 0, "viajes-lda"))
            {
                foreach (var doc in testDocs)
                {
                    if (lda.TryPredict(doc, out var topics))
                    {
                        var docTopics = string.Join("\n", topics.Select(t => lda.TryDescribeTopic(t.TopicID, out var td) ? $"[{t.Score:n3}] => {td.ToString()}" : ""));

                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine(doc.Value);
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine(docTopics);
                        Console.WriteLine("------------------------------------------\n\n");
                    }
                }
            }
        }
    }
}
