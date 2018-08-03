using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommentReview.Models;
using CommentReview.App_Code.Services;
using System.Text;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using static CommentReview.Models.YoutubeViewModel;
using CommentReview.Models.YoutubeResponseModel;
using CommentReview.Models.CommentsViewModel;
using System.Reflection;
using HtmlAgilityPack;
using ScrapySharp.Network;
using ScrapySharp.Extensions;

namespace CommentReview.Controllers
{
    public class Amazon
    {
        public string Star { get; set; }
    }
    

    public class HomeController : Controller
    {
        public byte[] tempdoc;
        private readonly YoutubeService _youtubeService;
        List<Amazon> amazonReviews = new List<Amazon>();

        public HomeController(YoutubeService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("amazonreviews")]
        public async Task<IActionResult> GetAmazonReviews()
        {
            var url = "https://www.amazon.com/Vinci-Code-Dan-Brown/product-reviews/0307474275/ref=cm_cr_dp_d_show_all_top?ie=UTF8&reviewerType=all_reviews";
            var result = await ProcessScrape(url);

            return Ok(result.Count);
        }

        public async Task<WebPage> Scrapper(string url)
        {
            ScrapingBrowser browser = new ScrapingBrowser();
            browser.AllowAutoRedirect = true;
            browser.AllowMetaRedirect = true;
            WebPage homePage = await browser.NavigateToPageAsync(new Uri(url));
            return homePage;
        }

        public async Task<IList<Amazon>> ProcessScrape(string url)
        {
            var baseUrl = "https://www.amazon.com/";
            var homePage = await Scrapper(url);
            var list = homePage.Html.CssSelect("#cm_cr-review_list div.review");
            var nextButton = homePage.Html.CssSelect("#cm_cr-pagination_bar ul.a-pagination > li.a-last").First();

            if (!nextButton.HasClass("a-disabled"))
            {
                var nextlink = nextButton.Element("a").ChildAttributes("href").First().Value;
                url = baseUrl + nextlink;
                foreach (var item in list.CssSelect("a.review-title"))
                {
                    amazonReviews.Add(
                        new Amazon() { Star = item.InnerText }
                        );
                }

                await ProcessScrape(url);
            }


                return amazonReviews;
         
           
        }

        [HttpGet]
        [Route("youtubecommentsjson/{link}/{token?}")]
        public async Task<IActionResult> GetYoutubeCommentsJson(string token, string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                return BadRequest("Link Empty");
            }
            try
            {
                var resultJson = await _youtubeService.GetComments(token, link);
                var resultClass = JsonConvert.DeserializeObject(resultJson);
                return Ok(JsonConvert.SerializeObject(resultClass));
            }
            catch (HttpRequestException e)
            {
                return Content("Error processing" + e);
            }
        }

        [HttpPost]
        [Route("export")]
        public void ExportToCsv([FromBody]IList<CommentsViewModel> model)
        {

            //string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            //var assetsDirectory = Directory.CreateDirectory(projectPath + @"\assets\");

            StringBuilder comments = new StringBuilder();

            //Add Header - Start
            comments.Append("Username")
                .Append(',')
                .Append("Date")
                .Append(',')
                .Append("Star rating")
                .Append(',')
                .Append("Comment")
                .Append(',')
                .Append("Link")
                .AppendLine();
            //Add Header - End

            //Add content body - start
            foreach (var item in model)
            {
                var comment = item.Snippet.TopLevelComment.Snippet.textDisplay.Replace(",", "");
                comments
              .Append(item.Snippet.TopLevelComment.Snippet.authorDisplayName)
              .Append(',')
              .Append(item.Snippet.TopLevelComment.Snippet.publishedAt)
              .Append(',')
              .Append(item.Snippet.TopLevelComment.Snippet.viewerRating)
              .Append(',')
              .Append(comment)
              .Append(',')
              .Append("https://www.youtube.com/"+item.Snippet.TopLevelComment.Snippet.videoId)
              .AppendLine();
            }
            //Add content body - End

            //Write to drive in case it's needed for record purposes
            //var path = assetsDirectory.ToString() + "YoutubeComments.csv";
            //var fileExist = System.IO.File.Exists(path);
            //if (fileExist)
            //{
            //    System.IO.File.Delete(path);
            //    System.IO.File.AppendAllText(path, comments.ToString());
            //}
            //else
            //{
            //    using (FileStream fs = System.IO.File.Create(path))
            //    {
            //        fs.Close();
            //        System.IO.File.AppendAllText(path, comments.ToString());
            //    }
            //}
         

            tempdoc = Encoding.ASCII.GetBytes(comments.ToString());
            //Change response type to force downaload
            HttpContext.Response.Clear();
            HttpContext.Response.Headers.Add("Content-Length", tempdoc.Length.ToString());
            HttpContext.Response.ContentType = "text/csv";
            HttpContext.Response.Headers.Add("content-disposition", "attachment;filename=YoutubeComments.csv");
            HttpContext.Response.Body.Write(tempdoc);

           
        }
    }
}
