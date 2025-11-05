using Microsoft.EntityFrameworkCore;
using MvcTaskNum2.Models;

namespace ToDoList518.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TodoTask> TodoTasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog= ToDoTask521 ;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;");

        }
    }
}

