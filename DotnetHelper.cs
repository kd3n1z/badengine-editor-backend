using System.Diagnostics;

namespace badengine_editor_backend;

public static class DotnetHelper {
    public static bool PrepareForAnalyse(string projectPath) => RunDotnet(projectPath, new[] { "build" });

    private static bool RunDotnet(string workingDirectory, IEnumerable<string> args) {
        Process process = new();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.WorkingDirectory = workingDirectory;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        foreach (string arg in args) {
            process.StartInfo.ArgumentList.Add(arg);
        }

        process.Start();
        process.WaitForExit();

        return process.ExitCode == 0;
    }
}