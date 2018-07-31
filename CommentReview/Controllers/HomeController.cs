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

namespace CommentReview.Controllers
{
    
    public class HomeController : Controller
    {
        public byte[] tempdoc;
        private readonly YoutubeService _youtubeService = new YoutubeService(new HttpClient());

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [Route("youtubecomments")]
        public IActionResult GetYoutubeComments(string token = "")
        {
            try
            {
                //var resultJson = await _youtubeService.GetComments(token);
                //var resultClass = YoutubeComments.FromJson(resultJson);
                return View();
            }
            catch (HttpRequestException e)
            {
                return Content("Error processing" + e);
            }
        }

        [Route("youtubecommentsjson/{token?}")]
        public async Task<IActionResult> GetYoutubeCommentsJson(string token)
        {
            try
            {
                var resultJson = await _youtubeService.GetComments(token);
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

            ////receive comments in JSON
            //var requestData = Request.HttpContext.Request.Body;

            ////Serialize comments to Class Object
            //var requestDataToJson = JsonConvert.SerializeObject(model);

            //var requestDataToClass = YoutubeResponseModel.FromJson(requestDataToJson);
            //var resultClass = JsonConvert.DeserializeObject<CommentsViewModel>(model);



            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var assetsDirectory = Directory.CreateDirectory(projectPath + @"\assets\");

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

            foreach (var item in model)
            {
               comments
              .Append(item.Snippet.TopLevelComment.Snippet.authorDisplayName)
              .Append(',')
              .Append(item.Snippet.TopLevelComment.Snippet.publishedAt)
              .Append(',')
              .Append(item.Snippet.TopLevelComment.Snippet.viewerRating)
              .Append(',')
              .Append(item.Snippet.TopLevelComment.Snippet.textDisplay)
              .Append(',')
              .Append("https://www.youtube.com/"+item.Snippet.TopLevelComment.Snippet.videoId)
              .AppendLine();

            }
            //Add content body - start

            //Write to drive in case it's needed for record purposes
            var path = assetsDirectory.ToString() + "YoutubeComments.csv";
            var fileExist = System.IO.File.Exists(path);
            if (fileExist)
            {
                System.IO.File.Delete(path);
                System.IO.File.AppendAllText(path, comments.ToString());
            }
            else
            {
                using (FileStream fs = System.IO.File.Create(path))
                {
                    fs.Close();
                    System.IO.File.AppendAllText(path, comments.ToString());
                }
            }
            //return Ok(model.ToList());

            tempdoc = Encoding.ASCII.GetBytes(comments.ToString());
            //Change response type to force downaload
            HttpContext.Response.Clear();
            HttpContext.Response.Headers.Add("Content-Length", tempdoc.Length.ToString());
            HttpContext.Response.ContentType = "text/csv";
            HttpContext.Response.Headers.Add("content-disposition", "attachment;filename=YoutubeComments.csv");
            HttpContext.Response.Body.Write(tempdoc);

            this.RedirectToAction(nameof(RedirectDownload), MethodBase.GetCurrentMethod().GetParameters().ToDictionary(x => x.Name, x => (object)null));
        }

        [Route("download")]
        public void RedirectDownload()
        {
            //Change response type to force downaload
            HttpContext.Response.Clear();
            HttpContext.Response.Headers.Add("content-disposition", "attachment;filename=YoutubeComments.csv");
            HttpContext.Response.Headers.Add("Content-Length", tempdoc.Length.ToString());
            HttpContext.Response.ContentType = "text/csv";
            HttpContext.Response.Body.Write(tempdoc);
        }
    }
}
