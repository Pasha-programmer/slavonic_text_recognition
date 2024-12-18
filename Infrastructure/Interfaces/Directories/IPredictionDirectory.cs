using Infrastructure.Models;

namespace Infrastructure.Interfaces.Directories;

public interface IPredictionDirectory
{
    Task<int> AddPrediction(PredictionDto model);
}
