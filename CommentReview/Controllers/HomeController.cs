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

namespace CommentReview.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            DownloadTodos();
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

        public void DownloadTodos()
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var assetsDirectory = Directory.CreateDirectory(projectPath + @"\assets\");

            var result = new List<TodoItem>()
            {
                new TodoItem{ Id = 1, Name = "Play Ball" },
                new TodoItem{ Id = 2, Name = "Play Soccer" },
                new TodoItem{ Id = 1, Name = "Play Video Game"},
                new TodoItem{ Id = 1, Name = "Play Tennis" }
            };

            StringBuilder todos = new StringBuilder();
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


            var path = assetsDirectory.ToString() +"user.xls";
            var fileExist = System.IO.File.Exists(path);
            if (fileExist)
            {
                System.IO.File.AppendAllText(path, todos.ToString());
            }
            else
            {
                using (FileStream fs = System.IO.File.Create(path))
                {
                    fs.Close();
                    System.IO.File.AppendAllText(path, todos.ToString());
                }
            }

        }

    }
}
