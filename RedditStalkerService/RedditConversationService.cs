using ChatModels;
using Reddit;
using Reddit.Controllers;
using Reddit.Exceptions;
using System.Collections.Generic;

namespace RedditStalkerService
{
    public class RedditConversationService
    {
        private readonly RedditClient reddit;
        public RedditConversationService(RedditClient redditClient)
        {
            reddit = redditClient;
        }

        public List<ConversationRequest> GetConversations(string userName)
        {
            var conversations = new List<ConversationRequest>();

            try
            {
                var user = reddit.User(userName);

                var commentHistory = user.CommentHistory;
                foreach (var comment in commentHistory)
                {
                    var conversation = new ConversationRequest
                    {
                        name = $"reddit-{comment.Fullname}",
                        type = "reddit",
                        responses = new List<AnalyzedChat>()
                    };
                    conversation.responses.Add(new AnalyzedChat { chat = new Chat { user = user.Name, message = comment.Body } });
                    conversation = GetConversation(conversation, comment);
                    conversations.Add(conversation);
                }
            }
            catch (RedditBadRequestException) { }
            catch (RedditNotFoundException) { }

            return conversations;
        }

        private ConversationRequest GetConversation(ConversationRequest conversation, Comment comment)
        {
            if (HasParentComment(comment))
            {
                try
                {
                    var parent = reddit.Comment(comment.ParentFullname).About();
                    conversation.responses.Insert(0, new AnalyzedChat { chat = new Chat { user = parent.Author, message = parent.Body } });
                    return GetConversation(conversation, parent);
                }
                catch (RedditNotFoundException) { }
            }
            else
            {
                try
                {
                    var parent = reddit.Post(comment.ParentFullname).About();
                    var message = parent.Title;
                    if (parent is SelfPost)
                    {
                        message += '\n' + (parent as SelfPost).SelfText;
                    }
                    if (parent is LinkPost)
                    {
                        message += '\n' + (parent as LinkPost).URL;
                    }
                    conversation.responses.Insert(0, new AnalyzedChat { chat = new Chat { user = parent.Author, message = message } });
                    return conversation;
                }
                catch (RedditNotFoundException) { }
            }

            return conversation;
        }

        private bool HasParentComment(Comment comment)
        {
            return comment.ParentFullname.StartsWith("t1_");
        }
    }
}
