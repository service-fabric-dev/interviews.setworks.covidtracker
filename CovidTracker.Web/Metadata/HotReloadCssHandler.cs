using System;
using System.Reflection.Metadata;
using System.Diagnostics;

//[assembly: MetadataUpdateHandler(typeof(HotReloadCssHandler))]
public static class HotReloadCssHandler
{
    public static void ClearCache(Type[]? updatedTypes)
    {
        // Optional: clear caches if needed
    }

    public static void UpdateApplication(Type[]? updatedTypes)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        RunTailwindRebuild();
    }

    private static void RunTailwindRebuild()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/c npx tailwindcss -i ./wwwroot/input.css -o ./wwwroot/output.css",
            WorkingDirectory = AppContext.BaseDirectory, // optional; could also use Directory.GetCurrentDirectory()
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        if (process != null)
        {
            process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (s, e) => Console.Error.WriteLine(e.Data);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }
    }
}
