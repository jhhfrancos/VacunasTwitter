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

namespace ProyectoIA
{
    public class MainClassLDA
    {
        public MainClassLDA()
        {
            Console.OutputEncoding = Encoding.UTF8;
            //ApplicationLogging.SetLoggerFactory(LoggerFactory.Create(lb => lb.AddConsole()));
        }

        public IEnumerable<IToken> Tokens(string texto)
        {
            FastTokenizer fastTokenizer = new FastTokenizer(Language.Spanish);
            var doc = new Document(texto, Language.Spanish);
            fastTokenizer.Process(doc);
            fastTokenizer.Parse(doc);
            return fastTokenizer.Parse(texto);
        }

        public async Task<(List<string>, List<string>)> Testing(List<string> stringArray)
        {
            ///////////-------------
            List<IDocument> testing = new List<IDocument>();
            foreach (var item in stringArray)
            {
                //var newItem = Utils.Utils.DeleteStopWords(item);
                testing.Add(new Document(item,Language.Spanish));
            }
            
            IDocument[] test = testing.ToArray();

            //Parse the documents using the English pipeline, as the text data is untokenized so far
            var nlp = Pipeline.For(Language.Spanish);
            ///////////////////----------------
            var testDocs = nlp.Process(test).ToArray();
            List<string> values = new List<string>();
            List<string> topicos = new List<string>();
            using (var lda = await LDA.FromStoreAsync(Language.Spanish, 0, "vacunas-lda"))
            {
                foreach (var doc in testDocs)
                {
                    if (lda.TryPredict(doc, out var topics))
                    {
                        var docTopics = string.Join("\n", topics.Select(t => lda.TryDescribeTopic(t.TopicID, out var td) ? $"\n [{t.Score:n3}] => {td.ToString()}" : ""));

                        lda.TryDescribeTopic(40, out var salida);

                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine(doc.Value);
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine(docTopics);
                        Console.WriteLine("------------------------------------------\n\n");
                        values.Add(doc.Value);
                        topicos.Add(docTopics);
                    }
                }
            }

            return (values, topicos);
        }

        public async Task<bool> Training(List<string> stringArray)
        {
            //Configures the model storage to use the online repository backed by the local folder ./catalyst-models/
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models-LDA"));

            //Download the Reuters corpus if necessary
            List<IDocument> training = new List<IDocument>();
            foreach (var item in stringArray)
            {
                //var newItem = Utils.Utils.DeleteStopWords(item);
                training.Add(new Document(item, Language.Spanish));
            }

            IDocument[] train = training.ToArray();

            //Parse the documents using the English pipeline, as the text data is untokenized so far
            var nlp = Pipeline.For(Language.Spanish);

            var trainDocs = nlp.Process(train).ToArray();
            
            //Train an LDA topic model on the trainind dateset
            using (var lda = new LDA(Language.Spanish, 0, "vacunas-lda"))
            {
                lda.Data.NumberOfTopics = 100; //Arbitrary number of topics
                lda.Train(trainDocs, Environment.ProcessorCount);

                await lda.StoreAsync();

                var stopWords = StopWords.Snowball.For(Language.Spanish);
            }

            return true;
        }

    }
}
