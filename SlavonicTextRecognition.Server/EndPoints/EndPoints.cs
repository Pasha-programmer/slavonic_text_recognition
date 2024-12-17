using Microsoft.AspNetCore.Mvc;
using SlavonicTextRecognition.Server.Contract;
using SlavonicTextRecognition.Server.Models;
using System.Text;
using TinyCsvParser;

namespace SlavonicTextRecognition.Server.EndPoints
{
    public static class EndPoints
    {
        public static void Init(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("api/process", async (
                [FromServices] IPythonApplication pythonApplication,
                IFormFile image) =>
            {
                await using var stream = image.OpenReadStream();

                var directoryPath = Path.Combine(Path.GetTempPath(), "input\\");
                var resultDirectoryPath = Path.Combine(Path.GetTempPath(), "prediction\\");

                Directory.CreateDirectory(directoryPath);
                Directory.CreateDirectory(resultDirectoryPath);

                var tempFile = Path.Combine(directoryPath, image.FileName.Replace(' ', '_'));

                await using (var fileStream = new FileStream(tempFile, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                };

                var resultFile = pythonApplication.Run(directoryPath, resultDirectoryPath)?.Trim();

                if (string.IsNullOrWhiteSpace(resultFile))
                    return Results.Problem();

                var lines = File.ReadAllLines(resultFile, Encoding.Default).Skip(1);

                var lineWords = lines.First()
                    .Split('\t');

                var prediction = new Prediction
                {
                    FileName = lineWords[0],
                    PredictionSize = lineWords[1],
                };

                Directory.Delete(directoryPath, true);
                Directory.Delete(resultDirectoryPath, true);

                return Results.Ok(prediction);
            }).DisableAntiforgery();
        }
    }
}
