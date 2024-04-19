using System.Text.Json;

namespace badengine_editor_backend; 

public static class JsonHelper {
    private static readonly JsonSerializerOptions? SerializerOptions = new() {
        IncludeFields = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string ToJson(object obj) {
        return JsonSerializer.Serialize(obj, SerializerOptions);
    }
}