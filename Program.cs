namespace badengine_editor_backend;

internal static class Program {
    public const string Version = "1.3.0";

    public static void Main(string[] args) {
        if (args.Length <= 0) {
            Console.WriteLine("oops! it looks like you are trying to use backend outside of the editor. " +
                              "for now, backend is not designed to work well outside of the editor, so use at your own risk.\n\n" +
                              "// maybe i will create a normal cli later"
            );

            Environment.ExitCode = 1;

            return;
        }

        string projectPath = args.Length >= 2 ? Path.GetFullPath(args[1]) : "";

        switch (args[0]) {
            case "getInfo":
                Console.WriteLine("backend v" + Version);
                return;
            case "version":
                Console.WriteLine(Version);
                return;
            case "watch":
                Logger.Log("watchStatus", "pretending to watch");
                while (true) {
                    Thread.Sleep(1000);
                }
            case "analyse":
                Logger.Log("analyseResult", Analyser.BuildAndAnalyse(projectPath).ToString());
                return;
            case "build":
                Logger.Log("buildStatus", "pretending to build");
                Thread.Sleep(4000);
                Logger.Log("buildStatus", "done");
                return;
            case "play":
                Logger.Log("buildStatus", "pretending to play");
                Thread.Sleep(2000);
                Logger.Log("playStatus", "game started");
                Thread.Sleep(5000);
                Logger.Log("playStatus", "game exited (code 0)");
                return;
            case "generateCsProj":
                Logger.Log("status", "generating game.csproj...");
                try {
                    File.WriteAllText(Path.Combine(projectPath, "game.csproj"), ProjectHelper.GenerateCsProj(args[2]));
                    Logger.Log("status", "done");
                }
                catch {
                    Logger.Log("status", "failed");
                }

                return;
            default:
                Console.WriteLine("unknown command: `" + args[0] + "`");
                return;
        }
    }
}