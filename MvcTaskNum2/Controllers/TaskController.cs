using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ToDoList518.Data;
using MvcTaskNum2.Models;

namespace MvcTaskNum2.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public TaskController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {
            var data = _db.TodoTasks.AsNoTracking().OrderByDescending(x => x.Id).ToList();
            ViewBag.UserName = Request.Cookies["TodoUserName"];
            return View(data);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(TodoTask todoTask)
        {
            return RedirectToAction(nameof(Index));  
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoTask model, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid data!";
                TempData["MessageType"] = "danger";
                return View(model);  
            }

            if (file != null && file.Length > 0)
            {
                model.FilePath = await SaveFileAsync(file);
            }

            _db.TodoTasks.Add(model);
            await _db.SaveChangesAsync();

            TempData["Message"] = "Task created successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Index)); 
        }
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _db.TodoTasks.FindAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoTask model, IFormFile? file)
        {
            if (id != model.Id)
            {
                TempData["Message"] = "Error: Invalid task ID!";
                TempData["MessageType"] = "danger";
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid data!";
                TempData["MessageType"] = "danger";
                return View(model);
            }

            var dbTask = await _db.TodoTasks.FindAsync(id);
            if (dbTask == null)
            {
                TempData["Message"] = "Task not found!";
                TempData["MessageType"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            dbTask.Title = model.Title;
            dbTask.Description = model.Description;
            dbTask.DeadLine = model.DeadLine;

            if (file != null && file.Length > 0)
            {
                if (!string.IsNullOrEmpty(dbTask.FilePath))
                    DeletePhysicalFile(dbTask.FilePath);

                dbTask.FilePath = await SaveFileAsync(file);
            }

            await _db.SaveChangesAsync();

            TempData["Message"] = "Task updated successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Index));  
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _db.TodoTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _db.TodoTasks.FindAsync(id);
            if (task == null)
            {
                TempData["Message"] = "Task not found!";
                TempData["MessageType"] = "danger";
                return RedirectToAction(nameof(Index));
            }

            if (!string.IsNullOrEmpty(task.FilePath))
                DeletePhysicalFile(task.FilePath);

            _db.TodoTasks.Remove(task);
            await _db.SaveChangesAsync();

            TempData["Message"] = "Task deleted successfully!";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Index)); 
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadsRoot = Path.Combine(_env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot"), "uploads");
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var ext = Path.GetExtension(file.FileName); 
            var newName = $"{Guid.NewGuid():N}{ext}";
            var physicalPath = Path.Combine(uploadsRoot, newName);

            using (var stream = System.IO.File.Create(physicalPath))
            {
                await file.CopyToAsync(stream);
            }

            var relative = $"/uploads/{newName}";
            return relative;
        }

        private void DeletePhysicalFile(string relativePath)
        {
            try
            {
                var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                var physical = Path.Combine(root, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(physical))
                    System.IO.File.Delete(physical);
            }
            catch {}
        }
    }
}
