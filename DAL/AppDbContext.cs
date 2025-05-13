using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Member> members { get; set; }
    }
}
