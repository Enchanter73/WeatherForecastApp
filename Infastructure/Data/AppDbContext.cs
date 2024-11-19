using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public required DbSet<City> Cities { get; set; }
        public required DbSet<Favorite> Favorites { get; set; }
        public required DbSet<User> Users {  get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Composite key to prevent duplicate favorite entries
            builder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.CityId })
                .IsUnique();
        }
    }
}
