using Microsoft.EntityFrameworkCore;
using SlavonicRecognition.Database;
using SlavonicRecognition.Infrastructure.Interfaces.Directories;
using SlavonicRecognition.Infrastructure.Models;

namespace SlavonicRecognition.Infrastructure.Services.Directories;

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
