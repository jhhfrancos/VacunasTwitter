﻿using MongoDB.Bson;
using ProyectoTesisBussiness.ML;
using ProyectoTesisDataAccess.Context;
using ProyectoTesisDataAccess.Repository;
using ProyectoTesisModels.Modelos;
using ProyectoTesisModels.Modelos.ForceDirectedGraph;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public IEnumerable<TableTopics> LDATweets(int limit)
        {
            var tweet = GetCleanTweets(limit);
            //var train = GetTweets(2000).Select(t => t.text).ToList();
            var train = tweet.Select(t => t.value.textoStop).ToList();
            var result = machinneLearnning.LDAAsync(train);
            // var resultTextClasification = machinneLearnning.TextClasification(train);
            List<TableTopics> returnValue = new List<TableTopics>();
            int index = 0;
            foreach (var item in result.Item1)
            {
                var tokens = machinneLearnning.Tokens(item);
                //var vec = machinneLearnning.WordToVec(new List<string>() { item });
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
                //var vec = machinneLearnning.WordToVec(new List<string>() { item });
                var dictionary = new TableTopics(item, result.Item2.ElementAt(index), tokens.ToArray());
                returnValue.Add(dictionary);
                index++;
            }
            return returnValue;
        }

        public IEnumerable<TableTopics> GetTweetNER(string text)
        {
            var result = machinneLearnning.NERTest(text);
            return result;
        }

        public IEnumerable<TableTopics> NERTweets(int limit)
        {
            var tweet = GetCleanTweets(limit);
            //var train = GetTweets(2000).Select(t => t.text).ToList();
            var train = tweet.Select(t => t.value.texto).ToList();
            var result = machinneLearnning.NER(train);
            return result;
            // var resultTextClasification = machinneLearnning.TextClasification(train);

        }

        public IEnumerable<FrequencyWord> WordCloud(int limit, string db)
        {
            var tweet = GetCleanTweets(limit, db);
            var train = tweet.Select(t => t.value.textoStop).ToList();
            List<TableTopics> returnValue = new List<TableTopics>();
            var vec = machinneLearnning.WordToVec(train);
            return vec;
        }

        public Graph ForcesGraph(int limit)
        {
            List<TweetClean> tweet = GetCleanTweets(limit, "Tweets_Base_Clean");
            //TODO: cantidad de relaciones t.Count()
            List<Node> nodesUser = tweet.GroupBy(t => t._id.idUser).Where(t => t.Count() > 4).Select(t => new Node() { id = t.Key.ToString(), group = "1" }).ToList();
            List<Node> nodesTweets = tweet.Where(t => nodesUser.Any(n => n.id == t._id.idUser.ToString())).Select(t => new Node() { id = t._id.idTweet.ToString(), group = "2" }).ToList();
            List<Node> nodes = nodesUser.Concat(nodesTweets).ToList();

            // tweet.Where(t => nodes.All(n => n.id == t._id.idTweet.ToString() || n.id == t._id.idUser.ToString())).ToList();
            List<Link> links = tweet.Where(t => nodes.Any(n => n.id == t._id.idTweet.ToString() || n.id == t._id.idUser.ToString())).Select(t => new Link() { source = t._id.idUser.ToString(), target = t._id.idTweet.ToString(), value = 1 }).ToList();

            Graph grafo = new Graph() { links = links, nodes = nodes };
            return grafo;
        }

        public bool ExecuteBash()
        {
            var result = $" ./twitter4j/auto-search.sh".ExecuteBash();
            return result;
        }

    }
}
