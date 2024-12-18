using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IProcessFilesService
{
    Task<bool> StartRecognizeWord(Uri directoryName);
}
