using System.Collections.Generic;
using ChatModels;
using Microsoft.AspNetCore.Mvc;
using RedditStalkerService;

namespace RedditStalkerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedditStalkerController : ControllerBase
    {
        private readonly RedditConversationService _conversationService;
        private readonly StalkerService _stalkerService;

        public RedditStalkerController(RedditConversationService redditStalkerService, StalkerService stalkerService)
        {
            _conversationService = redditStalkerService;
            _stalkerService = stalkerService;
        }

        /// <summary>
        /// returns a user's reddit conversations
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public IList<ConversationRequest> Get(string userName)
        {
            return _conversationService.GetConversations(userName);
        }

        /// <summary>
        /// gets a user's reddit conversations then sends them to the sharkbot api
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("stalk")]
        public void Stalk(string userName)
        {
            _stalkerService.Stalk(userName);
        }

        /// <summary>
        /// gets a all of a user's reddit conversations then sends them to the sharkbot api
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("deepstalk")]
        public void DeepStalk(string userName)
        {
            _stalkerService.Stalk(userName); //TODO: deep stalk and scheduling deep stalks
        }
    }
}
