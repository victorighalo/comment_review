using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CommentReview.App_Code.Helpers
{
    public class Network
    {
        protected static HttpClient _client;
        public Network()
        {
            _client = new HttpClient();
        }

        public static HttpClient SendAction(string baseUrl = "")
        {
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();

            return _client;
        }
    }
}
