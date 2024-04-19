namespace badengine_editor_backend;

public static class Logger {
    private struct Message {
        // ReSharper disable NotAccessedField.Local
        public string Type;
        public string Data;
    }

    public static void Log(string type, string data) {
        Console.WriteLine(JsonHelper.ToJson(new Message { Type = type, Data = data }));
    }
}