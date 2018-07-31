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
        private readonly HttpClient _client = new HttpClient();

        public YoutubeService()
        {
            _client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<string> GetComments(string token, string link)
        {
            var response = await _client.GetAsync("commentThreads?part=snippet%2Creplies&maxResults=100&videoId="+link+"&fields=items(snippet(topLevelComment(snippet(authorDisplayName%2CpublishedAt%2CtextDisplay%2CtextOriginal%2CvideoId%2CviewerRating))%2CvideoId))%2CnextPageToken%2CpageInfo%2CtokenPagination&maxResults=100&pageToken=" + token + "&key=AIzaSyCzy_i2tptiVyvoIeMqVDSXZBH0_J3abLI");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
