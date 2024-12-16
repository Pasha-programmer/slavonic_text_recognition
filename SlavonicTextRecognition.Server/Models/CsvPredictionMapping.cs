using TinyCsvParser.Mapping;

namespace SlavonicTextRecognition.Server.Models;

public class CsvPredictionMapping : CsvMapping<Prediction>
{
    public CsvPredictionMapping()
        : base()
    {
        MapProperty(0, x => x.FileName);
        MapProperty(1, x => x.PredictionSize);
    }
}
