using Infrastructure.Models;
using Infrastructure.Models.Enums;

namespace Infrastructure.Interfaces;

public interface IFilePredicationService
{
    Task<IReadOnlyCollection<FilePredictionInfoDto>> GetFilePredications(DateTime? fromDate, DateTime? toDate, StatusEnum? status = null);
}
