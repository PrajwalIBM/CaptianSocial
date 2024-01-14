using CaptianSocial.Data;
using CaptianSocial.Models;
using CaptianSocial.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaptianSocial.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDBContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        public CommentController(ApplicationDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SubmitComment(AddCommentViewModel commentRequest)
        {
            if (commentRequest != null)
            {
                // Access the properties like reactionViewModel.PostId, reactionViewModel.ReactionType
                // Perform the necessary actions, update the database, etc.
                try
                {
                    //check if this user has already reacted to this post before.
                    var session = httpContextAccessor.HttpContext.Session;
                    if (session != null)
                    {
                        var newComment = new Comment
                        {
                            PostId = commentRequest.PostId,
                            UserId = Guid.Parse(session.GetString("userid")),
                            Message = commentRequest.Message
                        };
                        await dbContext.Comments.AddAsync(newComment);
                        await dbContext.SaveChangesAsync();
                        // You can return a response if needed
                        return Json(new { success = true, message = "User Comment Submitted successfully" });
                    }
                    return RedirectToAction("Login", "Authentication");

                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal Server Error!" + ex);
                }
            }

            // Return a response in case of an error
            return Json(new { success = false, message = "Invalid data received" });
        }
    }
}
