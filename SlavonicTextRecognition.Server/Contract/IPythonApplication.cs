namespace SlavonicTextRecognition.Server.Contract;

public interface IPythonApplication
{
    string? Run(params string[] agrs);
}
