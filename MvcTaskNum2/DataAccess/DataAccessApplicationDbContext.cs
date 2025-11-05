using Microsoft.EntityFrameworkCore;
using MvcTaskNum2.Models;

namespace ToDoList518.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<TodoTask> TodoTasks { get; set; }
    }
}
