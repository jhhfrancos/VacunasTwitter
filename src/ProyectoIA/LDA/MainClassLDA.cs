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
using ProyectoTesisModels;
using ProyectoTesisModels.Modelos;
using static Catalyst.Models.LDA;

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
            //Configures the model storage to use the online repository backed by the local folder ./catalyst-models/
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models-LDA"));
            ///////////-------------
            List<IDocument> testing = new List<IDocument>();
            foreach (var item in stringArray)
            {
                //var newItem = Utils.Utils.DeleteStopWords(item);
                testing.Add(new Document(item, Language.Spanish));
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
            //var topicos2 = GetTopics();
            return (values, topicos);
        }

        public async Task<List<TopicModel>> TestingModel(List<string> stringArray)
        {
            //Configures the model storage to use the online repository backed by the local folder ./catalyst-models/
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models-LDA"));
            List<Document> testing = stringArray.Select(i => new Document(i,Language.Spanish)).ToList();
            
            var nlp = Pipeline.For(Language.Spanish);
            var testDocs = nlp.Process(testing).ToArray();

            List<TopicModel> topicsModel = new List<TopicModel>();

            using (var lda = await LDA.FromStoreAsync(Language.Spanish, 0, "vacunas-lda"))
            {
                foreach (var doc in testDocs)
                {
                    var topicModel = new TopicModel()
                    {
                        Document = doc.Value,
                        TopicDescriptions = new List<TopicModelDescription>()
                    };
                    if (lda.TryPredict(doc, out var topics))
                    {
                        foreach (var topicModelPredicted in topics)
                        {
                            if (lda.TryDescribeTopic(topicModelPredicted.TopicID, out var td))
                            {
                                topicModel.TopicDescriptions.Add(
                                    new TopicModelDescription()
                                    {
                                        Score = topicModelPredicted.Score,
                                        Tokens = (Dictionary<string, float>)td.Tokens,
                                        TopicID = td.TopicID
                                    });
                            }
                        }
                    }
                    topicsModel.Add(topicModel);
                }
            }
            return topicsModel;
        }

        public async Task<int> getNumberOfTopics()
        {
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models-LDA"));

            using (var lda = await LDA.FromStoreAsync(Language.Spanish, 0, "vacunas-lda"))
            {
                return lda.Data.NumberOfTopics; ;
            }
        }

        public async Task<List<LDATopicDescription>> GetTopics()
        {
            //Configures the model storage to use the online repository backed by the local folder ./catalyst-models/
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models-LDA"));

            using (var lda = await LDA.FromStoreAsync(Language.Spanish, 0, "vacunas-lda"))
            {
                var numTopics = lda.Data.NumberOfTopics;
                var topics = new List<LDATopicDescription>();
                for (int i = 0; i < numTopics; i++)
                {
                    lda.TryDescribeTopic(i, out var salida);
                    topics.Add(salida);
                }

                return topics;
            }

        }

        public async Task<bool> Training(List<string> stringArray, int numTopics, int NumOfTerms)
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
                lda.Data.NumberOfTopics = numTopics; //Arbitrary number of topics
                lda.Data.NumberOfSummaryTermsPerTopic = NumOfTerms; //Arbitrary number of topics
                lda.Train(trainDocs, Environment.ProcessorCount);

                await lda.StoreAsync();

                var stopWords = StopWords.Snowball.For(Language.Spanish);
            }

            return true;
        }

    }
}
