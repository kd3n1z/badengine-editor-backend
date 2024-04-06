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
            default:
                Console.WriteLine("unknown command: `" + args[0] + "`");
                return;
        }
    }
}