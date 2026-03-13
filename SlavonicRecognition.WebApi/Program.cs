using Microsoft.EntityFrameworkCore;
using SlavonicRecognition.Database;
using SlavonicRecognition.Infrastructure.Interfaces;
using SlavonicRecognition.Infrastructure.Interfaces.Directories;
using SlavonicRecognition.Infrastructure.Services;
using SlavonicRecognition.Infrastructure.Services.Directories;
using SlavonicTextRecognition.Server.EndPoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IPredicateImagesService, PredicateImagesService>();
builder.Services.AddScoped<IProcessFilesService, ProcessFilesService>();
builder.Services.AddScoped<IDocumentDirectory, DocumentDirectory>();
builder.Services.AddScoped<IPredictionDirectory, PredictionDirectory>();
builder.Services.AddScoped<ITaskOperationDirectory, TaskOperationDirectory>();
builder.Services.AddScoped<IFilePredicationService, FilePredicationService>();

builder.Services.AddDbContextFactory<Context>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Context>();
    db.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

EndPoints.Init(app);

app.MapFallbackToFile("/index.html");

app.Run();
