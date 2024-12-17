using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IProcessFiles
    {
        Task<Uri?> Process(Uri directoryName);

        Task<IReadOnlyCollection<Prediction>> GetPrediction(Uri resultProcessFile);
    }
}
