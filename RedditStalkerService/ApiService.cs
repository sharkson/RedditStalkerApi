using ChatModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RedditStalkerService
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SendConversationsToApi(List<ConversationRequest> conversations)
        {
            foreach(var conversation in conversations)
            {
                var jsonString = JsonConvert.SerializeObject(conversation);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                _httpClient.PutAsync($"api/ConversationImport/", content);
            }
        }
    }
}
