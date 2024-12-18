using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Directories;
using Infrastructure.Models;
using System.Text;

namespace Infrastructure.Services;

public class ProcessFilesService(
    IPredicateImagesService pythonApplication,
    IDocumentDirectory documentDirectory,
    ITaskOperationDirectory taskOperationDirectory,
    IPredictionDirectory predictionDirectory
    ) : IProcessFilesService
{
    public async Task<bool> StartRecognizeWord(Uri directoryName)
    {
        var directoryNameString = directoryName.ToString();

        var processFiles = Directory.GetFiles(directoryNameString);

        var documents = new Dictionary<string, int>(processFiles.Length);

        foreach (var file in processFiles)
        {
            var blob = File.ReadAllBytes(file);

            var fileName = Path.GetFileName(file);

            var documentId = await documentDirectory.AddDocument(new()
            {
                FileName = fileName,
                FileBlob = blob
            });

            var taskId = await taskOperationDirectory.AddTaskOperation(new()
            {
                DocumentId = documentId,
                Status = Models.Enums.StatusEnum.Process,
            });

            documents.Add(fileName, documentId);
        }

        var resultDirectoryPath = Path.Combine(Path.GetTempPath(), DateTime.Now.ToString() + "\\");

        var resultFile = pythonApplication.Run(directoryNameString, resultDirectoryPath)?.Trim();

        // если не обработалось
        if (string.IsNullOrWhiteSpace(resultFile))
        {
            foreach (var file in processFiles)
            {
                var fileName = Path.GetFileName(file);

                var taskId = await taskOperationDirectory.AddTaskOperation(new()
                {
                    DocumentId = documents[fileName],
                    Status = Models.Enums.StatusEnum.Error,
                });
            }

            Directory.Delete(directoryNameString, true);

            return false;
        }

        var predictions = await GetPredictionWord(new Uri(resultFile));
        var predictionMap = predictions.ToDictionary(x => x.FileName, x => x.PredictionWord);

        foreach (var file in processFiles)
        {
            var fileName = Path.GetFileName(file);

            if (!predictionMap.ContainsKey(fileName))
            {
                _ = await taskOperationDirectory.AddTaskOperation(new()
                {
                    DocumentId = documents[fileName],
                    Status = Models.Enums.StatusEnum.Error,
                });
            }

            var predictionId = await predictionDirectory.AddPrediction(new()
            {
                DocumentId = documents[fileName],
                PredictionWord = predictionMap[fileName]
            });

            var taskId = await taskOperationDirectory.AddTaskOperation(new()
            {
                DocumentId = documents[fileName],
                Status = Models.Enums.StatusEnum.Success,
            });
        }

        Directory.Delete(directoryNameString, true);

        return true;
    }

    internal async Task<IReadOnlyCollection<PredictionCsvDto>> GetPredictionWord(Uri resultProcessFile)
    {
        var resultProcessFileString = resultProcessFile.ToString();

        var lines = (await File.ReadAllLinesAsync(resultProcessFileString, Encoding.Default))
            .Skip(1);

        var predictions = lines.Select(x => x.Split('\t'))
            .Select(lineWords => new PredictionCsvDto
            {
                FileName = lineWords[0],
                PredictionWord = lineWords[1],
            })
            .ToArray();

        Directory.Delete(resultProcessFileString, true);

        return predictions;
    }
}
