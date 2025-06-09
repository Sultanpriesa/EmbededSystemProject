using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions options) :
    base(options)
    {

    }
    public DbSet<Movement>? Movements { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movement>().HasKey(m => m.MoveID);
        modelBuilder.Entity<Movement>().HasData(
        new Movement()
        {
            MoveID = 1,
            Message = "You Got Mail!",
            ButtonSignal = 1,
            SensorSignal = 1,
        }
        );
    }
}