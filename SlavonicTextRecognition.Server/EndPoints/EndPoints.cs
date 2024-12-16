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

                var resultFile = pythonApplication.Run(directoryPath, resultDirectoryPath);

                var csvParserOptions = new CsvParserOptions(true, '\t');
                var csvParser = new CsvParser<Prediction>(csvParserOptions, new CsvPredictionMapping());
                var result = csvParser
                             .ReadFromFile(resultFile.Trim(), Encoding.UTF32)
                             .ToArray();

                Directory.Delete(directoryPath, true);
                Directory.Delete(resultDirectoryPath, true);

                return result.Select(x => x.Result).ToArray();
            }).DisableAntiforgery();

            //endpointRouteBuilder.MapGet("api/process", async (
            //    [FromServices] IPythonApplication pythonApplication,
            //    string a, string b) =>
            //{
            //    return pythonApplication.Run(a, b);
            //}).DisableAntiforgery();
        }
    }
}
