using ProyectoTesisDataAccess.Context;
using ProyectoTesisDataAccess.Repository;
using ProyectoTesisModels.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoTesisBussiness.BussinessControllers
{
    public class TweetsProfilesBussiness
    {
        private TwitterProfilesRepository twitterProfilesRepository;
        public TweetsProfilesBussiness()
        {
            twitterProfilesRepository = new TwitterProfilesRepository();
        }

        public Tweet GetFirstTweet()
        {
            return twitterProfilesRepository.GetFirstTweet(); ;
        }
        public List<Tweet> GetTweets(int limit)
        {
            return twitterProfilesRepository.GetAllTweets(limit);
        }
        
    }
}
