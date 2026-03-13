using Microsoft.EntityFrameworkCore;
using SlavonicRecognition.Database;
using SlavonicRecognition.Infrastructure.Interfaces.Directories;
using SlavonicRecognition.Infrastructure.Models;

namespace SlavonicRecognition.Infrastructure.Services.Directories;

public class DocumentDirectory(
    IDbContextFactory<Context> contextFactory
    ) : IDocumentDirectory
{
    public async Task<int> AddDocument(DocumentDto model)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var document = new Document
        {
            FileName = model.FileName,
            FileBlob = model.FileBlob,
            CreateAt = DateTime.Now,
        };

        var entity = await context.AddAsync(document);

        await context.SaveChangesAsync();

        return entity.Entity.Id;
    }
}
