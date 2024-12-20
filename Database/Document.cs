namespace Database;

public class Document
{
    public int Id { get; set; }

    public string FileName {  get; set; }

    public DateTime CreateAt { get; set; }

    public byte[] FileBlob {  get; set; }
}
