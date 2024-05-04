using System.Reflection;

namespace badengine_editor_backend;

public static class Analyser {
    public static AnalyseResult BuildAndAnalyse(string projectPath) {
        LogStatus("building `" + projectPath + "`...");

        if (!DotnetHelper.PrepareForAnalyse(projectPath)) {
            return new AnalyseResult("failed", "unable to build `" + projectPath + "`");
        }

        string dllPath = Path.Combine(projectPath, "bin", "Debug", "net6.0", "game.dll");

        LogStatus("loading `" + dllPath + "`...");

        Assembly gameAssembly;

        try {
            gameAssembly = Assembly.LoadFrom(dllPath);
        }
        catch {
            return new AnalyseResult("failed", "unable to load assembly `" + dllPath + "`");
        }

        string badengineDllPath = Path.Combine(dllPath, "../badengine.dll");

        LogStatus("loading `" + badengineDllPath + "`...");

        Assembly badengineAssembly;

        try {
            badengineAssembly = Assembly.LoadFrom(badengineDllPath);
        }
        catch {
            return new AnalyseResult("failed", "unable to load assembly `" + badengineDllPath + "`");
        }

        LogStatus("getting basic types...");

        Type? badengineComponentType = badengineAssembly.GetType("Badengine.Component");

        if (badengineComponentType == null) {
            return new AnalyseResult("failed", "unable to find Badengine.Component type");
        }

        Type? badengineAlwaysActiveAttributeType = badengineAssembly.GetType("Badengine.Attributes.AlwaysActive");

        if (badengineAlwaysActiveAttributeType == null) {
            return new AnalyseResult("failed", "unable to find Badengine.Attributes.AlwaysActive type");
        }

        Type? badengineNonRegistrableAttributeType = badengineAssembly.GetType("Badengine.Attributes.NonRegistrable");

        if (badengineNonRegistrableAttributeType == null) {
            return new AnalyseResult("failed", "unable to find Badengine.Attributes.NonRegistrable type");
        }

        return new AnalyseResult("ok", JsonHelper.ToJson(AnalyseAssembly(
            gameAssembly.GetTypes().Concat(badengineAssembly.GetTypes()),
            badengineComponentType,
            badengineAlwaysActiveAttributeType,
            badengineNonRegistrableAttributeType
        )));
    }

    private static AnalyseAssemblyInfo AnalyseAssembly(
        IEnumerable<Type> allTypes,
        Type badengineComponentType,
        Type badengineAlwaysActiveAttributeType,
        Type badengineNonRegistrableAttributeType) {
        AnalyseAssemblyInfo info = new();

        foreach (Type type in allTypes) {
            if (type is { IsClass: true, IsAbstract: false } && type.IsSubclassOf(badengineComponentType)) {
                object? instance = null;

                if (type.GetConstructors().Any(c => c.GetParameters().Length <= 0)) {
                    instance = Activator.CreateInstance(type);
                }


                info.Components.Add(new AnalyseAssemblyInfo.AnalyseComponentInfo(type.FullName!,
                    type.GetFields()
                        .Where(e => e.IsPublic)
                        .Select(e => new AnalyseAssemblyInfo.AnalyseComponentInfo.AnalyseFieldInfo(e.Name, e.FieldType.FullName!,
                            e.FieldType.IsValueType && instance != null ? TypeToString(e.GetValue(instance)) : null)).ToArray(),
                    type.GetCustomAttributes(badengineAlwaysActiveAttributeType, true).Length > 0,
                    type.GetCustomAttributes(badengineNonRegistrableAttributeType, true).Length > 0
                ));
            }
        }

        return info;
    }

    private static string? TypeToString(object? obj) {
        return obj switch {
            null => null,
            float or double => obj.ToString()!.Replace(",", "."),
            _ => obj.ToString()
        };
    }

    private static void LogStatus(string message) => Logger.Log("analyseStatus", message);

    public class AnalyseResult {
        public string Status; // "ok" or "failed"
        public string Data;

        public AnalyseResult(string status, string data) {
            Status = status;
            Data = data;
        }

        public override string ToString() => JsonHelper.ToJson(this);
    }

    public class AnalyseAssemblyInfo {
        public class AnalyseComponentInfo {
            public class AnalyseFieldInfo {
                public string Type;
                public string Name;
                public string? DefaultValue;

                public AnalyseFieldInfo(string name, string type, object? defaultValue = null) {
                    Name = name;
                    Type = type;

                    if (defaultValue != null) {
                        DefaultValue = defaultValue.ToString();
                    }
                }
            }

            public string Name;
            public bool AlwaysActive;
            public bool NonRegistrable;
            public AnalyseFieldInfo[] Fields;

            public AnalyseComponentInfo(string name, AnalyseFieldInfo[] fields, bool alwaysActive, bool nonRegistrable) {
                Fields = fields;
                Name = name;
                AlwaysActive = alwaysActive;
                NonRegistrable = nonRegistrable;
            }
        }

        // ReSharper disable once CollectionNeverQueried.Global
        public readonly List<AnalyseComponentInfo> Components = new();

        public override string ToString() => JsonHelper.ToJson(this);
    }
}