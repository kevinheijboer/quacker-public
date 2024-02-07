using AuthService.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<ApplicationUser> ApplicationUser { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}
	}
}
