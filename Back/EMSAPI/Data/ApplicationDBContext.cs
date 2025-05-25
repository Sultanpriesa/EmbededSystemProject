using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Movement>? Movements { get; set; }

    public ApplicationDbContext(DbContextOptions options) :
    base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movement>().HasData(
        new Movement()
        {
            MoveID = 1,
            Message = "You Got Mail!",
            ButtonSignal = 1,
        }
        );
    }
}