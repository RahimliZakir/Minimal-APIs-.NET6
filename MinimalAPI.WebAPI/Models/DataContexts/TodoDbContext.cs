using Microsoft.EntityFrameworkCore;
using MinimalAPI.WebAPI.Models.Entities;

namespace MinimalAPI.WebAPI.Models.DataContexts
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Todo> Todos { get; set; }
    }
}
