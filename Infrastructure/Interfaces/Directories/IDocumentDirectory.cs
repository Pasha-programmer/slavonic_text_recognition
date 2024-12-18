using Infrastructure.Models;

namespace Infrastructure.Interfaces.Directories;

public interface IDocumentDirectory
{
    Task<int> AddDocument(DocumentDto model);
}
