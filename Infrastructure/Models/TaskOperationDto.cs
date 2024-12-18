using Infrastructure.Models.Enums;

namespace Infrastructure.Models;

public record TaskOperationDto
{
    public StatusEnum Status { get; set; }

    public int DocumentId { get; set; }
}
