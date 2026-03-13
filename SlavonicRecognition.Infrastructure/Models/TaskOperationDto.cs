using SlavonicRecognition.Infrastructure.Models.Enums;

namespace SlavonicRecognition.Infrastructure.Models;

public record TaskOperationDto
{
    public StatusEnum Status { get; set; }

    public int DocumentId { get; set; }
}
