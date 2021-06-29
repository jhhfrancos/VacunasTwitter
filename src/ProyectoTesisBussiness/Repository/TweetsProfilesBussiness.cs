using ProyectoTesisBussiness.ML;
using ProyectoTesisDataAccess.Context;
using ProyectoTesisDataAccess.Repository;
using ProyectoTesisModels.Modelos;
using ProyectoTesisModels.Modelos.ForceDirectedGraph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TwitterRetrieval;

namespace ProyectoTesisBussiness.BussinessControllers
{
    public class TweetsProfilesBussiness
    {
        private TwitterProfilesRepository twitterProfilesRepository;
        MachinneLearnning machinneLearnning;
        public TweetsProfilesBussiness()
        {
            twitterProfilesRepository = new TwitterProfilesRepository();
            machinneLearnning = new MachinneLearnning();
        }

        public Tweet GetFirstTweet()
        {
            return twitterProfilesRepository.GetFirstTweet(); ;
        }
        public List<Tweet> GetTweets(int limit)
        {
            return twitterProfilesRepository.GetAllTweets(limit);
        }

        public List<TweetClean> GetCleanTweets(int limit, string name = "Tweets_Profiles_Clean")
        {
            return twitterProfilesRepository.GetCleanTweets(limit, name);
        }


        public bool LDATrainTweets(int limit, int numTopics, int NumOfTerms)
        {
            var tweet = GetCleanTweets(limit);
            //var train = GetTweets(2000).Select(t => t.text).ToList();
            var train = tweet.Select(t => t.value.textoStop).ToList();
            var result = machinneLearnning.LDATrainAsync(train, numTopics, NumOfTerms);
            return result;
        }


        public IEnumerable<TableTopics> LDATestResultTweets(int limit)
        {
            var tweet = GetCleanTweets(limit, "Tweets_Base_Clean");
            var test = tweet.Select(t => t.value.textoStop).ToList();
            var result = machinneLearnning.LDATestResultAsync(test);
            List<TableTopics> returnValue = new List<TableTopics>();
            int index = 0;
            foreach (var item in result.Item1)
            {
                var tokens = machinneLearnning.Tokens(item);
                var dictionary = new TableTopics(item, result.Item2.ElementAt(index), tokens.ToArray());

                returnValue.Add(dictionary);
                index++;
            }
            return returnValue;
        }
        public IEnumerable<TableTopics> GetTweetLDA(string text)
        {
            var result = machinneLearnning.LDATest(text);
            List<TableTopics> returnValue = new List<TableTopics>();
            int index = 0;
            foreach (var item in result.Item1)
            {
                var tokens = machinneLearnning.Tokens(item);
                //var vec = machinneLearnning.FrequencyWords(new List<string>() { item });
                var dictionary = new TableTopics(item, result.Item2.ElementAt(index), tokens.ToArray());
                returnValue.Add(dictionary);
                index++;
            }
            return returnValue;
        }
        public IEnumerable<TopicModel> GetTweetLDAModel(int limit)
        {
            var tweets = GetCleanTweets(limit, "Tweets_Base_clean");
            var result = machinneLearnning.TestingModel(tweets.Select(t => t.value.textoStop).ToList());
            return result;
        }

        public TableTopics GetTweetNER(string text)
        {
            var result = machinneLearnning.NERTest(text);
            return result;
        }

        public async Task<bool> NERTrainTweetsAsync(int limit)
        {
            var tweet = GetCleanTweets(limit);
            var train = tweet.Select(t => t.value.texto).ToList();
            var result = machinneLearnning.NERAsync(train);
            return await result;

        }

        public IEnumerable<TableTopics> NERTestResultTweets(int limit)
        {
            var tweets = GetCleanTweets(limit, "Tweets_Base_clean");
            var test = tweets.Select(t => machinneLearnning.NERTest(t.value.texto));
            return test.ToList();
        }


        public IEnumerable<FrequencyWord> WordCloud(int limit, string db)
        {
            var tweet = GetCleanTweets(limit, db);
            var train = tweet.Select(t => t.value.textoStop).ToList();
            List<TableTopics> returnValue = new List<TableTopics>();
            var vec = machinneLearnning.FrequencyWords(train);
            return vec;
        }

        public Graph ForcesGraph(int limit, string db)
        {
            List<TweetClean> tweet = GetCleanTweets(limit, db);
            //TODO: cantidad de relaciones t.Count()
            List<Node> nodesUser = tweet.GroupBy(t => t._id.idUser).Where(t => t.Count() > 4).Select(t => new Node() { id = t.Key.ToString(), group = "1" }).ToList();
            List<Node> nodesTweets = tweet.Where(t => nodesUser.Any(n => n.id == t._id.idUser.ToString())).Select(t => new Node() { id = t._id.idTweet.ToString(), group = "2" }).ToList();
            List<Node> nodes = nodesUser.Concat(nodesTweets).ToList();

            // tweet.Where(t => nodes.All(n => n.id == t._id.idTweet.ToString() || n.id == t._id.idUser.ToString())).ToList();
            List<Link> links = tweet.Where(t => nodes.Any(n => n.id == t._id.idTweet.ToString() || n.id == t._id.idUser.ToString())).Select(t => new Link() { source = t._id.idUser.ToString(), target = t._id.idTweet.ToString(), value = 1 }).ToList();

            Graph grafo = new Graph() { links = links, nodes = nodes };
            return grafo;
        }

        public TSNEResponse Tsnegraphics(int limit, int perplexity)
        {
            int numberOfTopics = machinneLearnning.getNumberOfTopics();
            var documents = GetTweetLDAModel(limit);
            var countDocuments = documents.Count();
            double[][] inputsTSNE = new double[countDocuments][];
            int[] targetsTSNE = new int[countDocuments];
            //Fullfill with Zeros the input matrix
            Array.Clear(inputsTSNE, 0, inputsTSNE.Length);
            for (int i = 0; i < countDocuments; i++)
            {
                var document = documents.ElementAt(i);
                var inputRow = inputsTSNE[i] = new double[numberOfTopics];
                Array.Clear(inputsTSNE[i], 0, numberOfTopics);
                document.TopicDescriptions.ForEach(item => inputRow[item.TopicID] = item.Score);
                targetsTSNE[i] = document.TopicDescriptions.FirstOrDefault().TopicID;
            }
            var response = machinneLearnning.CreateTSNEModel(inputsTSNE, targetsTSNE, perplexity);
            response.documents = documents.ToList();
            return response;
        }

        public List<Catalyst.Models.LDA.LDATopicDescription> GetAllTopics()
        {
            return machinneLearnning.GetAllTopics();
        }

        public bool ExecuteBash()
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var fullPath = Path.GetFullPath(@".//twitter4j//auto-search.sh");
            //var result = @"D:\Maestria\Tesis\Vacunas\TwitterVacunas\VacunasTwitter\src\ProyectoTesis\twitter4j\auto-search.sh".ExecuteBash();
            var result = fullPath.ExecuteBash();
            return result;
        }

    }
}
