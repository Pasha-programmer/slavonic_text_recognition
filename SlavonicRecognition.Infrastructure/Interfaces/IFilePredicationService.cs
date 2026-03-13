using SlavonicRecognition.Infrastructure.Models;
using SlavonicRecognition.Infrastructure.Models.Enums;

namespace SlavonicRecognition.Infrastructure.Interfaces;

public interface IFilePredicationService
{
    Task<IReadOnlyCollection<FilePredictionInfoDto>> GetFilePredications(DateTime? fromDate, DateTime? toDate, StatusEnum? status = null);
}
