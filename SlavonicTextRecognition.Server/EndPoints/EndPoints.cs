using Microsoft.AspNetCore.Mvc;
using SlavonicTextRecognition.Server.Contract;

namespace SlavonicTextRecognition.Server.EndPoints
{
    public static class EndPoints
    {
        public static void Init(IEndpointRouteBuilder endpointRouteBuilder)
        {
            //endpointRouteBuilder.MapGet("api/process", async (
            //    [FromServices] IPythonApplication pythonApplication,
            //    IFormFile image) =>
            //{
            //    await using var stream = image.OpenReadStream();

            //    var fileName = Path.GetRandomFileName();
            //    var pathToSave = Path.Combine(Path.GetTempPath(), fileName);

            //    await using var fileStream = new FileStream(pathToSave, FileMode.Create);
            //    await stream.CopyToAsync(fileStream);

            //    pythonApplication.Run(pathToSave);
            //}).DisableAntiforgery();

            endpointRouteBuilder.MapGet("api/process", async (
                [FromServices] IPythonApplication pythonApplication,
                string a, string b) =>
            {
                return pythonApplication.Run(a, b);
            }).DisableAntiforgery();
        }
    }
}
