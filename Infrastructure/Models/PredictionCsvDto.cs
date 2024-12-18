namespace Infrastructure.Models;

public record PredictionCsvDto
{
    public string FileName { get; set; }

    public string PredictionWord { get; set; }
}
