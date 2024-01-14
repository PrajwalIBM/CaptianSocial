using Azure.Core;
using CaptianSocial.Data;
using CaptianSocial.Models;
using CaptianSocial.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaptianSocial.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDBContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        public PostController(ApplicationDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddPost()
        {
            var session = httpContextAccessor.HttpContext.Session;
            if (session.GetString("username") != null)
            {
                var user = new User
                {
                    Id = Guid.Parse(session.GetString("userid")),
                    Name = session.GetString("username")
                };
                return View("AddPost");
            }
            return RedirectToAction("Login", "Authentication");
        }
        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostViewModel request)
        {
            var session = httpContextAccessor.HttpContext.Session;
            var post = new Post
            {
                AuthorName = session.GetString("username"),
                AuthorId = Guid.Parse(session.GetString("userid")),
                ImageURL = request.ImageURL,
                Description = request.Description,
                PublishedDate = DateTime.Now
            };
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
            ViewBag.Username = session.GetString("username");

            return RedirectToAction("Feed", "Feed");
        }
    }
}
