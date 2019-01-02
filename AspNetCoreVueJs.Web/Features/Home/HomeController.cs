using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreVueJs.Web.Models;
using Microsoft.Extensions.Logging;
using AspNetCoreVueJs.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace AspNetCoreVueJs.Web.Features
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EcommerceContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly  List<SeedUser> _seedUsers;

        public HomeController(ILogger<HomeController> logger,
            EcommerceContext dbContext,
            IConfiguration configuration, 
            IHostingEnvironment hostingEnvironment,
            IOptions<List<SeedUser>> seedUsers)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _seedUsers = seedUsers.Value;
        }
        public IActionResult Index()
        {
            string path = Directory.GetCurrentDirectory();
           // TempData["Message"] = $"The current directory is {path}";
          
            _logger.LogWarning($"The current directory is {path}");
            var dbpath = _dbContext.Database.ProviderName;
            _logger.LogInformation(dbpath);
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
        [Authorize(Policy = "RequireAdministratorRole")]
        public IActionResult SiteLog()
        {
            var logDir = "";
            // var file = _configuration["Serilog:WriteTo[0]:Args:Path"];
            var file = _configuration["Serilog:WriteTo:0:Args:path"];
            if (string.IsNullOrEmpty(file))
            {
                return NoContent();
            }
            if (Path.IsPathFullyQualified(file))
            {
              logDir =   Path.GetDirectoryName(file);
            }
            else
            {
                file = Path.Combine(_hostingEnvironment.WebRootPath, file);
                logDir = Path.GetDirectoryName(file);
            }
            DirectoryInfo dir = new DirectoryInfo(logDir);

            string lastFile = dir.GetFiles()
                .Where(x=>x.Name.StartsWith("VueEcommerce"))
                .OrderByDescending(p => p.LastWriteTime).FirstOrDefault().FullName;



            Stream stream = System.IO.File.Open(lastFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        
            StreamReader streamReader = new StreamReader(stream);
            string fileContents = streamReader.ReadToEnd();
           
            streamReader.Close();
            stream.Close();

            ViewData["Log"] = fileContents;

            return View();
        }

        public IActionResult SeedDatabase()
        {
            _logger.LogInformation("Seeding databse");
            _dbContext.Database.Migrate();
            _dbContext.EnsureSeeded(_seedUsers);
            _logger.LogInformation("Seeding complete!");
            return RedirectToAction(nameof(Privacy));
        }
    }
}
