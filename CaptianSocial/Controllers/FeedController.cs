using CaptianSocial.Data;
using CaptianSocial.Models;
using CaptianSocial.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaptianSocial.Controllers
{
    public class FeedController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDBContext dbContext;
        public FeedController(ApplicationDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Feed(User user)
        {
            var posts = await dbContext.Posts.ToListAsync();
            var session = httpContextAccessor.HttpContext.Session;
            if(session.GetString("username") != null)
            {
                var userId = Guid.Parse(session.GetString("userid"));
                var reactionsForPost = await dbContext.Reactions.SingleOrDefaultAsync(r => r.UserID == userId);

                // Pass the data to the view
                ViewBag.ReactionsForPost = reactionsForPost;
                ViewBag.Username = session.GetString("username");
                return View(posts);
            }
            else
            {
                return RedirectToAction("Login", "Authentication");
            }
        }
        public async Task<IActionResult> PostDetails(Guid postId)
        {
            // Retrieve reactions data for the user and post from your database
            var session = httpContextAccessor.HttpContext.Session;
            var userId = Guid.Parse(session.GetString("user_id"));
            var reactionsForPost = await dbContext.Reactions.SingleOrDefaultAsync(r => r.UserID == userId && r.PostID == postId);

            // Pass the data to the view
            ViewBag.ReactionsForPost = reactionsForPost;

            return View();
        }
    }
}
