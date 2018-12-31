using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreVueJs.Web.Models;
using Microsoft.Extensions.Logging;
using AspNetCoreVueJs.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AspNetCoreVueJs.Web.Features
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EcommerceContext _dbContext;

        public HomeController(ILogger<HomeController> logger, EcommerceContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
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

        public IActionResult SeedDatabase()
        {
            _logger.LogInformation("Seeding databse");
            _dbContext.Database.Migrate();
            _dbContext.EnsureSeeded();
            _logger.LogInformation("Seeding complete!");
            return RedirectToAction(nameof(Privacy));
        }
    }
}
