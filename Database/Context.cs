using Microsoft.EntityFrameworkCore;

namespace Database;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> optionsBuilder)
        : base(optionsBuilder)
    { }

    public DbSet<Document> Documents { get; set; }

    public DbSet<TaskProcess> Tasks { get; set; }
    
    public DbSet<Prediction> Predictions { get; set; }
}
