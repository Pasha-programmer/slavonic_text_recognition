using Infrastructure.Interfaces;
using System.Diagnostics;
using System.Reflection;

namespace Infrastructure.Services;

public class PredicateImages : IPythonApplication
{
    public string? Run(params string[] agrs)
    {
        try
        {
            // Получение относительного пути к скрипту Python
            var pythonScriptPath = FindPythonScript("Python", "main.py");

            // Проверка наличия скрипта Python
            if (string.IsNullOrEmpty(pythonScriptPath) || !File.Exists(pythonScriptPath))
            {
                Console.WriteLine($"Ошибка: скрипт Python не найден по пути: {pythonScriptPath}");
                return null;
            }

            // Путь к интерпретатору Python в виртуальном окружении
            var pythonInterpreterPath = FindPythonInterpreter("Python", "myenv", "Scripts", "python.exe");

            // Проверка наличия интерпретатора Python
            if (string.IsNullOrEmpty(pythonInterpreterPath) || !File.Exists(pythonInterpreterPath))
            {
                Console.WriteLine($"Ошибка: интерпретатор Python не найден по пути: {pythonInterpreterPath}");
                return null;
            }

            // Настройка процесса для вызова скрипта Python
            var start = new ProcessStartInfo
            {
                FileName = pythonInterpreterPath,
                Arguments = $"{pythonScriptPath} {agrs.Aggregate((x, y) => $"{x} {y}")}",
                WorkingDirectory = Path.GetDirectoryName(pythonScriptPath),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true, // Захват стандартного вывода ошибок
                CreateNoWindow = true,
            };

            using var process = Process.Start(start)
                ?? throw new Exception($"Процесс пустой: {start}");

            using var reader = process.StandardOutput;

            var result = reader.ReadToEnd();

            Console.WriteLine($"Результат: {result}");

            using var errorReader = process.StandardError;

            var error = errorReader.ReadToEnd();
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Ошибка: {error}");
            }

            process.WaitForExit();
            var exitCode = process.ExitCode;

            Console.WriteLine($"Процесс завершился с кодом: {exitCode}");

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Исключение: {ex.Message}");
            Console.WriteLine($"Стек вызовов: {ex.StackTrace}");
        }

        return null;
    }

    private string? FindPythonScript(string directory, string scriptName)
    {
        return FindFile(directory, scriptName);
    }

    private string? FindPythonInterpreter(string directory, string venvDirectory, string scriptsDirectory, string pythonExe)
    {
        return FindFile(directory, Path.Combine(venvDirectory, scriptsDirectory, pythonExe));
    }

    private string? FindFile(string directory, string fileName)
    {
        // Получение начального каталога сборки
        var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        while (currentDirectory != null)
        {
            var foundPath = SearchDirectory(currentDirectory, directory, fileName);
            if (foundPath == null)
            {
                // Переход в родительский каталог
                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
                continue;
            }

            return foundPath;
        }

        // Если файл не найден, вернуть null
        return null;
    }

    private string? SearchDirectory(string baseDirectory, string targetDirectory, string fileName)
    {
        // Получение полного пути к целевой директории
        var targetPath = Path.Combine(baseDirectory, targetDirectory);

        if (!Directory.Exists(targetPath))
            return null;

        var filePath = Path.Combine(targetPath, fileName);
        if (File.Exists(filePath))
        {
            return filePath;
        }

        // Рекурсивный поиск в подкаталогах
        foreach (var subDirectory in Directory.GetDirectories(targetPath))
        {
            var foundPath = SearchDirectory(subDirectory, string.Empty, fileName);
            if (foundPath == null)
                continue;

            return foundPath;
        }

        return null;
    }
}
