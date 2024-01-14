using CaptianSocial.Data;
using CaptianSocial.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CaptianSocial.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDBContext dbContext;
        public HomeController(ILogger<HomeController> logger, ApplicationDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

/*        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public IActionResult Index()
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("username") != null)
            {
                return RedirectToAction("Feed", "Feed");
            }
            else
            {
                return View();
            }
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
