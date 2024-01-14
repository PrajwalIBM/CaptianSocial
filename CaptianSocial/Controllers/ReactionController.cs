using Azure.Core;
using CaptianSocial.Data;
using CaptianSocial.Models;
using CaptianSocial.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaptianSocial.Controllers
{
    public class ReactionController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDBContext dbContext;
        public ReactionController(ApplicationDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SubmitReaction([FromBody] AddReactionViewModel reactionViewModel)
        {
            if (reactionViewModel != null)
            {
                // Access the properties like reactionViewModel.PostId, reactionViewModel.ReactionType
                // Perform the necessary actions, update the database, etc.
                try {
                    //check if this user has already reacted to this post before.
                    var reaction = await dbContext.Reactions.SingleOrDefaultAsync(
                        r => r.UserID == reactionViewModel.UserId && r.PostID == reactionViewModel.PostId);
                    if (reaction != null) {
                        // If this If is executed then this user has already reacted to this post before.
                        // In this case we just update the user ReactionId.
                        try
                        {
                            reaction.ReactionType = reactionViewModel.ReactionType;
                            await dbContext.SaveChangesAsync();
                        }catch (Exception ex)
                        {
                            return Json(new { success = false, message = "Something happened while saving changes." + ex });
                        }
                        return Json(new { success = true, message = "User Reaction Updated successfully" });
                    }
                    else
                    {
                        var session = httpContextAccessor.HttpContext.Session;
                        if(session != null)
                        {
                            var newReaction = new Reaction
                            {
                                PostID = reactionViewModel.PostId,
                                UserID = Guid.Parse(session.GetString("userid")),
                                ReactionType = reactionViewModel.ReactionType
                            };
                            await dbContext.Reactions.AddAsync(newReaction);
                            await dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            return RedirectToAction("Login", "Authentication");
                        }
                        // You can return a response if needed
                        return Json(new { success = true, message = "User Reaction Submitted successfully" });
                    }
                } catch (Exception ex) {
                    Console.Error.WriteLine(ex.ToString());
                    return StatusCode(500, "Internal Server Error, something didn't go accorinding to plan!");
                }
            }

            // Return a response in case of an error
            return Json(new { success = false, message = "Invalid data received" });
        }
    }
}
