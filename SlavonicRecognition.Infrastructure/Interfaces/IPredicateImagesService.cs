namespace SlavonicRecognition.Infrastructure.Interfaces;

public interface IPredicateImagesService
{
    Task<string?> Run(params string[] agrs);
}
