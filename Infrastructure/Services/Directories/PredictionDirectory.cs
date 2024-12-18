using Database;
using Infrastructure.Interfaces.Directories;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Directories;

public class PredictionDirectory(
    IDbContextFactory<Context> contextFactory
    ) : IPredictionDirectory
{
    public async Task<int> AddPrediction(PredictionDto model)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var prediction = new Prediction
        {
            Value = model.PredictionWord,
            DocumentId = model.DocumentId,
        };

        var entity = await context.AddAsync(prediction);

        await context.SaveChangesAsync();

        return entity.Entity.Id;
    }
}
