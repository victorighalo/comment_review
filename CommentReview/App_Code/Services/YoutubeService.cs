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
        private readonly HttpClient _client;

        public YoutubeService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/");

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        public async Task<string> GetComments(string token)
        {
            var response = await _client.GetAsync("commentThreads?part=snippet%2Creplies&maxResults=100&videoId=iublAXAm8HQ&fields=items(snippet(topLevelComment(snippet(authorDisplayName%2CpublishedAt%2CtextDisplay%2CtextOriginal%2CvideoId%2CviewerRating))%2CvideoId))%2CnextPageToken%2CpageInfo%2CtokenPagination&maxResults=100&pageToken=" + token + "&key=AIzaSyCzy_i2tptiVyvoIeMqVDSXZBH0_J3abLI");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
