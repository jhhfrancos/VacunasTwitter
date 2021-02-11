using ProyectoTesisBussiness.ML;
using ProyectoTesisDataAccess.Context;
using ProyectoTesisDataAccess.Repository;
using ProyectoTesisModels.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public List<TweetClean> GetCleanTweets(int limit)
        {
            return twitterProfilesRepository.GetCleanTweets(limit);
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

        public IEnumerable<TableTopics> NERTweets(int limit)
        {
            var tweet = GetCleanTweets(limit);
            //var train = GetTweets(2000).Select(t => t.text).ToList();
            var train = tweet.Select(t => t.value.texto).ToList();
            var result = machinneLearnning.LDAAsync(train);
            // var resultTextClasification = machinneLearnning.TextClasification(train);
            List<TableTopics> returnValue = new List<TableTopics>();
            int index = 0;
            //var vec = machinneLearnning.WordToVec(result.Item1);
            foreach (var item in result.Item1)
            {
                var tokens = machinneLearnning.Tokens(item);
                var dictionary = new TableTopics(item, result.Item2.ElementAt(index), tokens.ToArray());

                returnValue.Add(dictionary);
                index++;
            }
            return returnValue;
        }

        public IEnumerable<FrequencyWord> WordCloud(int limit)
        {
            var tweet = GetCleanTweets(limit);
            var train = tweet.Select(t => t.value.textoStop).ToList();
            List<TableTopics> returnValue = new List<TableTopics>();
            var vec = machinneLearnning.WordToVec(train);
            return vec;
        }
    }
}
