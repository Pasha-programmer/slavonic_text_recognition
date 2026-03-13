using Microsoft.EntityFrameworkCore;

namespace SlavonicRecognition.Database;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> optionsBuilder)
        : base(optionsBuilder)
    { }

    public DbSet<Document> Documents { get; set; }

    public DbSet<TaskProcess> Tasks { get; set; }
    
    public DbSet<Prediction> Predictions { get; set; }
}
