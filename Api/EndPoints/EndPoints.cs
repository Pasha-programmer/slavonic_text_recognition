using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SlavonicTextRecognition.Server.EndPoints;

public static class EndPoints
{
    public static void Init(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("api/documents/process/upload", async (
            [FromServices] IProcessFilesService processFilesService,
            [FromForm] IFormFile image) =>
        {
            var directoryPath = new Uri(Path.Combine(Path.GetTempPath(), "input\\"));
            var resultDirectoryPath = new Uri(Path.Combine(Path.GetTempPath(), "prediction\\"));

            Directory.CreateDirectory(directoryPath.ToString());

            var tempFile = Path.Combine(directoryPath.ToString(), image.FileName.Replace(' ', '_'));

            await using (var stream = image.OpenReadStream())
            await using (var fileStream = new FileStream(tempFile, FileMode.Create))
                await stream.CopyToAsync(fileStream);

            var isSuccess = await processFilesService.StartRecognizeWord(directoryPath);

            if (!isSuccess)
                return Results.Problem();

            return Results.Ok();
        }).DisableAntiforgery();
    }
}
