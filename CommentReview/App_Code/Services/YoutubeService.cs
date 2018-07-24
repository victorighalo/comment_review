using CommentReview.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CommentReview.App_Code.Services
{
    public class YoutubeService
    {
        public HttpClient Client { get; }

        public YoutubeService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/");

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client = client;
        }

        public async Task<string> GetComments()
        {
            var response = await Client.GetAsync("commentThreads?part=snippet%2Creplies&videoId=wtLJPvx7-ys&key=AIzaSyCzy_i2tptiVyvoIeMqVDSXZBH0_J3abLI");

            response.EnsureSuccessStatusCode();



            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
