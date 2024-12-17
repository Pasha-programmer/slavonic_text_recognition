namespace Infrastructure.Interfaces;

public interface IPythonApplication
{
    string? Run(params string[] agrs);
}
