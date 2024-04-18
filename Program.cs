namespace badengine_editor_backend;

internal static class Program {
    public const string Version = "1.0.0";

    public static void Main(string[] args) {
        switch (args[0]) {
            case "getInfo":
                Console.WriteLine("backend v" + Version);
                return;
            case "version":
                Console.WriteLine(Version);
                return;
            case "watch":
                Console.WriteLine("pretending to watch " + args[1]);
                while (true) {
                    Thread.Sleep(1000);
                }
                return;
            case "build":
                Console.WriteLine("pretending to build " + args[1]);
                Thread.Sleep(4000);
                return;
            case "play":
                Console.WriteLine("pretending to start " + args[1]);
                Thread.Sleep(2000);
                return;
            default:
                Console.WriteLine("unknown command: `" + args[0] + "`");
                return;
        }
    }
}