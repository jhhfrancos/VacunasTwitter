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

        public IEnumerable<TableTopics> LDATweets()
        {
            var train = GetTweets(2000).Select(t => t.text).ToList();
            var result = machinneLearnning.LDAAsync(train);
            var resultTextClasification = machinneLearnning.TextClasification(train);
            List<TableTopics> returnValue = new List<TableTopics>();
            int index = 0;
            foreach (var item in result.Item1)
            {
                var tokens = machinneLearnning.Tokens(item);
                var vec = machinneLearnning.WordToVec(item);
                var dictionary = new TableTopics(item, result.Item2.ElementAt(index), vec.ToArray());
                
                returnValue.Add(dictionary);
                index++;
            }

            
            return returnValue;
        }
        
    }
}
