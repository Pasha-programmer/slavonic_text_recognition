namespace Infrastructure.Models;

public record PredictionDto
{
    public int DocumentId { get; set; }

    public string PredictionWord { get; set; }
}
