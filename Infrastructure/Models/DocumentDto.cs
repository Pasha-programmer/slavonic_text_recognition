namespace Infrastructure.Models;

public record DocumentDto
{
    public string FileName { get; set; }

    public byte[] FileBlob { get; set; }
}
