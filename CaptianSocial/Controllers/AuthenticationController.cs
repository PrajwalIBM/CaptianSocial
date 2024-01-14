using CaptianSocial.Data;
using CaptianSocial.Models;
using CaptianSocial.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaptianSocial.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDBContext dbContext;
        public AuthenticationController(ApplicationDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel request)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Name == request.Name && u.Password == request.Password);
            if (user != null)
            {
                var session = httpContextAccessor.HttpContext.Session;
                session.SetString("username", user.Name);
                session.SetString("userid", user.Id.ToString());

                return RedirectToAction("Feed", "Feed");
            }
            else
            {
                // Id or Password was Incorrect.
                return View();
            }
        }
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel request)
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            // Now find the userID and userName and create the session with the credentials.
            var newUser = await dbContext.Users.SingleAsync(u => u.Name == request.Name);

            // If the user is saved successfully then, we will be able to find the users. Creating a Session Now!
            var session = httpContextAccessor.HttpContext.Session;
            session.SetString("username", newUser.Name);
            session.SetString("userid", newUser.Id.ToString());

            return RedirectToAction("Feed", "Feed");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Authentication");
        }
    }
}
