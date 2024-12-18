using Database;
using Infrastructure.Interfaces.Directories;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Directories;

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
        };

        var entity = await context.AddAsync(document);

        await context.SaveChangesAsync();

        return entity.Entity.Id;
    }
}
