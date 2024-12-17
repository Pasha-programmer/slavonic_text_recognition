using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Text;

namespace Infrastructure.Services;

public class ProcessFiles(
    IPythonApplication pythonApplication
    ) : IProcessFiles
{
    public async Task<IReadOnlyCollection<Prediction>> GetPrediction(Uri resultProcessFile)
    {
        var resultProcessFileString = resultProcessFile.ToString();

        var lines = (await File.ReadAllLinesAsync(resultProcessFileString, Encoding.Default))
            .Skip(1);

        var prediction = lines.Select(x => x.Split('\t'))
            .Select(lineWords => new Prediction
            {
                FileName = lineWords[0],
                PredictionSize = lineWords[1],
            })
            .ToArray();

        Directory.Delete(resultProcessFileString, true);

        return prediction;
    }

    public async Task<Uri?> Process(Uri directoryName)
    {
        var directoryNameString = directoryName.ToString();

        var resultDirectoryPath = Path.Combine(Path.GetTempPath(), DateTime.Now.ToString() + "\\");

        var resultFile = pythonApplication.Run(directoryNameString, resultDirectoryPath)?.Trim();

        Directory.Delete(directoryNameString, true);

        if (string.IsNullOrWhiteSpace(resultFile))
            return null;

        return new Uri(resultFile);
    }
}
