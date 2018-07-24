using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommentReview.Models;
using CommentReview.App_Code.Helpers;
using System.Text;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace CommentReview.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient _network = new HttpClient();

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

        public void LoadUrl()
        {
            var request = Network.SendAction("http://www.nairaland.com");
            request.GetAsync("/");
        }

        public class TodoItem
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public bool IsComplete { get; set; }
        }

        public void ExportToCsv()
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var assetsDirectory = Directory.CreateDirectory(projectPath + @"\assets\");

            var result = new List<TodoItem>()
            {
                new TodoItem{ Id = 1, Name = "Play Ball" },
                new TodoItem{ Id = 2, Name = "Play Soccer" },
                new TodoItem{ Id = 1, Name = "Play Video Game"},
                new TodoItem{ Id = 1, Name = "Play Tennis" }
            };

            HttpContext.Response.Clear();
            HttpContext.Response.Headers.Add("content-disposition", "attachment;filename = ExportedCSV.csv");
            
            HttpContext.Response.ContentType = "text/csv";

            StringBuilder todos = new StringBuilder();

            //Add Header - Start
            todos.Append("Id")
                .Append(',')
                .Append("Name")
                .Append(',')
                .Append("Done");
            todos.AppendLine();
            //Add Header - End

            //Add content body - start
            foreach (var record in result)
            {
                todos
                .Append(record.Id)
                .Append(',')
                .Append(record.Name)
                .Append(',')
                .Append(record.IsComplete)
               .Append(Environment.NewLine);
            }
            //Add content body - start

            //Write to drive in case it's needed for record purposes
            //var path = assetsDirectory.ToString() +"user.csv";
            //var fileExist = System.IO.File.Exists(path);
            //if (fileExist)
            //{
            //    System.IO.File.Delete(path);
            //    System.IO.File.AppendAllText(path, todos.ToString());
            //}
            //else
            //{
            //    using (FileStream fs = System.IO.File.Create(path))
            //    {
            //        fs.Close();
            //        System.IO.File.AppendAllText(path, todos.ToString());
            //    }
            //}
            HttpContext.Response.WriteAsync(todos.ToString());
            
        }

    }
}
