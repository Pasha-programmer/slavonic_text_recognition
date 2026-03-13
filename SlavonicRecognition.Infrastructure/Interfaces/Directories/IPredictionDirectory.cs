using SlavonicRecognition.Infrastructure.Models;

namespace SlavonicRecognition.Infrastructure.Interfaces.Directories;

public interface IPredictionDirectory
{
    Task<int> AddPrediction(PredictionDto model);
}
