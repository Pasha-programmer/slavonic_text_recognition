using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SlavonicTextRecognition.Server.EndPoints;

public static class EndPoints
{
    public static void Init(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("api/documents/process/upload", async (
            [FromServices] IProcessFilesService processFilesService,
            [FromForm] IFormFile images) =>
        {
            var directoryPath = new Uri(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "\\"));

            Directory.CreateDirectory(directoryPath.AbsolutePath);

            var tempFile = Path.Combine(directoryPath.AbsolutePath, images.FileName.Replace(' ', '_'));

            await using (var stream = images.OpenReadStream())
            await using (var fileStream = new FileStream(tempFile, FileMode.Create))
                await stream.CopyToAsync(fileStream);

            var isSuccess = await processFilesService.StartRecognizeWord(directoryPath);

            if (!isSuccess)
                return Results.Problem();

            return Results.Ok();
        }).DisableAntiforgery();
    }
}
