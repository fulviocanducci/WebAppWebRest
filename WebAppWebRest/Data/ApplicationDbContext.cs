using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppWebRest.Models;

namespace WebAppWebRest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Todos> Todos { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new TodosConfiguration());
        }
    }
}
