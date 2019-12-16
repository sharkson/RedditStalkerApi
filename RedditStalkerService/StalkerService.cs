using System;
using System.Collections.Generic;

namespace RedditStalkerService
{
    public class StalkerService
    {
        private readonly RedditConversationService _redditStalkerService;
        private readonly ApiService _apiService;
        private readonly int _stalkFrequencyMinutes;

        private Dictionary<string, DateTime> stalkTimes;

        public StalkerService(RedditConversationService redditStalkerService, ApiService apiService)
        {
            _redditStalkerService = redditStalkerService;
            _apiService = apiService;

            _stalkFrequencyMinutes = 60;

            stalkTimes = new Dictionary<string, DateTime>();
        }

        public bool ShouldStalk(string userName)
        {
            if (stalkTimes.ContainsKey(userName))
            {
                var stalkTime = stalkTimes[userName];
                if ((DateTime.Now - stalkTime).TotalMinutes > _stalkFrequencyMinutes)
                {
                    stalkTimes[userName] = DateTime.Now;
                    return true;
                }
            }
            else
            {
                stalkTimes.Add(userName, DateTime.Now);
                return true;
            }

            return false;
        }

        public void Stalk(string userName)
        {
            if (ShouldStalk(userName))
            {
                var conversations = _redditStalkerService.GetConversations(userName);
                _apiService.SendConversationsToApi(conversations);
            }
        }
    }
}
