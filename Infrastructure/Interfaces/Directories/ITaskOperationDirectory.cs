using Infrastructure.Models;

namespace Infrastructure.Interfaces.Directories;

public interface ITaskOperationDirectory
{
    Task<int> AddTaskOperation(TaskOperationDto model);
}
