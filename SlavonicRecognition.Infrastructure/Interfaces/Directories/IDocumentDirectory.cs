using SlavonicRecognition.Infrastructure.Models;

namespace SlavonicRecognition.Infrastructure.Interfaces.Directories;

public interface IDocumentDirectory
{
    Task<int> AddDocument(DocumentDto model);
}
