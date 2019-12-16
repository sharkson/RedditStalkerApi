using RedditStalkerService;
using Xunit;

namespace RedditStalkerServiceTests
{
    public class RedditConversationTests
    {
        RedditConversationService service;

        public RedditConversationTests()
        {
            service = new RedditConversationService(new Reddit.RedditClient(appId: "YOURAPPID", appSecret: "YOURAPPSECRET", refreshToken: "YOURREFRESHTOKEN"));
        }

        [Fact]
        public void NormalUser()
        {
            var conversations = service.GetConversations("sharknice");
            Assert.Equal(25, conversations.Count);
        }

        [Fact]
        public void EmptyUser()
        {
            var conversations = service.GetConversations("");
            Assert.Empty(conversations);
        }

        [Fact]
        public void FakeUser()
        {
            var conversations = service.GetConversations("asdfasdfasdfasdfasdfasdfasdfasdf");
            Assert.Empty(conversations);
        }
    }
}
