using Infrastructure.Interfaces;
using Infrastructure.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace SlavonicTextRecognition.Server.EndPoints;

public static class EndPoints
{
    public static void Init(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("api/documents/process/upload", async (
            [FromServices] IProcessFilesService processFilesService,
            [FromForm] IFormFileCollection images) =>
        {
            var directoryPath = new Uri(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "\\"));

            Directory.CreateDirectory(directoryPath.AbsolutePath);

            foreach (var formFile in images)
            {
                var tempFile = Path.Combine(directoryPath.AbsolutePath, formFile.FileName.Replace(' ', '_'));

                await using var stream = formFile.OpenReadStream();
                await using var fileStream = new FileStream(tempFile, FileMode.Create);
                await stream.CopyToAsync(fileStream);
            }

            var isSuccess = await processFilesService.StartRecognizeWord(directoryPath);

            if (!isSuccess)
                return Results.Problem();

            return Results.Ok();
        }).DisableAntiforgery();

        endpointRouteBuilder.MapGet("api/documents", async (
           [FromServices] IFilePredicationService filePredicationService,
           DateTime? fromDate, DateTime? toDate, StatusEnum? status) =>
            {
                return await filePredicationService.GetFilePredications(fromDate, toDate, status);
            });
    }
}
