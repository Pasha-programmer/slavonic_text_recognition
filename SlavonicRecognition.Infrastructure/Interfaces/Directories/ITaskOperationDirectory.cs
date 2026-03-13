using SlavonicRecognition.Infrastructure.Models;

namespace SlavonicRecognition.Infrastructure.Interfaces.Directories;

public interface ITaskOperationDirectory
{
    Task<int> AddTaskOperation(TaskOperationDto model);
}
