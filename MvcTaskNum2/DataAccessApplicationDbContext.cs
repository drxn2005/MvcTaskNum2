using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace MvcTaskNum2
{
    public class DataAccessApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MvcTaskNum2Db;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}