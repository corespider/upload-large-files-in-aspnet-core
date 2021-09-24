using CP.UploadLargeFiles_aspnetcore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CP.UploadLargeFiles_aspnetcore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IWebHostEnvironment Environment { get; private set; }

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment _environment)
        {
            _logger = logger;
            Environment = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public IActionResult Index(List<IFormFile> postedFiles)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;
            string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    postedFile.CopyTo(stream);
                }
            }
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
    }
}
