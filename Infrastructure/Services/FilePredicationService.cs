using Database;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class FilePredicationService(
    IDbContextFactory<Context> dbContextFactory
    ) : IFilePredicationService
{
    public async Task<IReadOnlyCollection<FilePredictionInfoDto>> GetFilePredications(DateTime? fromDate, DateTime? toDate, StatusEnum? status)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var query = from d in context.Documents
                    join t in context.Tasks on d.Id equals t.DocumentId

                    let prediction = (from p in context.Predictions
                                       where p.DocumentId == d.Id && t.Status == (int)StatusEnum.Success
                                       select p.Value).SingleOrDefault()

                    let taskStatuses = (from t1 in context.Tasks
                                        where t1.DocumentId == d.Id 
                                        select t1.Status).ToList()

                    where t.Status == (int)StatusEnum.Success || t.Status == (int)StatusEnum.Error || taskStatuses.Count == 1

                    select new
                    {
                        DocumentId = d.Id,
                        FileName = d.FileName,
                        Content = prediction,
                        Status = t.Status,
                        CreateAt = d.CreateAt,
                    };

        if (fromDate.HasValue)
            query = query.Where(x => x.CreateAt >= fromDate);

        if (toDate.HasValue)
            query = query.Where(x => x.CreateAt < toDate);

        if (status.HasValue)
            query = query.Where(x => x.Status == (int)status.Value);

        return await query.Select(x => new FilePredictionInfoDto
        {
            DocumentId = x.DocumentId,
            FileName = x.FileName,
            Content = x.Content,
        }).ToArrayAsync();
    }
}
