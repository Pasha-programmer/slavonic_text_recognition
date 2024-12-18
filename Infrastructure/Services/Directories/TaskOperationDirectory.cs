using Database;
using Infrastructure.Interfaces.Directories;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Directories;

public class TaskOperationDirectory(
    IDbContextFactory<Context> contextFactory
    ) : ITaskOperationDirectory
{
    public async Task<int> AddTaskOperation(TaskOperationDto model)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var task = new TaskProcess
        {
            DocumentId = model.DocumentId,
            Status = (int)model.Status,
        };

        var entity = await context.AddAsync(task);

        await context.SaveChangesAsync();

        return entity.Entity.Id;
    }
}
