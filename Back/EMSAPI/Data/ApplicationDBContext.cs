using Microsoft.EntityFrameworkCore;
using EMSAPI.Models;

namespace EMSAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {
        }

        public DbSet<Movement> Movements { get; set; } = default!;
          protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movement>().HasKey(m => m.MoveID);
            
            // Seed data - for seeding, we need to provide the ID explicitly
            modelBuilder.Entity<Movement>().HasData(
                new Movement()
                {
                    MoveID = 1, // Required for seeding
                    Message = "You Got Mail!",
                    ButtonSignal = 1,
                    SensorSignal = 1,
                    DataFrom = "Frontend"
                }
            );
        }
    }
}