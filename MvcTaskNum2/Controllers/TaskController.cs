using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MvcTaskNum2.Models;
using ToDoList518.Data;

namespace MvcTaskNum2.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TaskController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var data = _db.TodoTasks.ToList();
            ViewBag.UserName = Request.Cookies["TodoUserName"]; 
            return View(data);
        }

        // GET: /Task/CreateUser
        public IActionResult CreateUser() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Response.Cookies.Append(
                    "TodoUserName",
                    name,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(1), 
                        HttpOnly = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.Lax,
                        Secure = Request.IsHttps 
                    }
                );
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
