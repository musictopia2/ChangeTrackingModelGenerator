namespace ChangeTrackingModelGenerator;
internal static class CodeBlockExtensions
{
    public static ICodeBlock PopulateGetChangesMethod(this ICodeBlock w, ResultsModel result)
    {
        w.WriteLine("public global::CommonBasicLibraries.CollectionClasses.BasicList<string> GetChanges()")
            .WriteCodeBlock(w =>
            {
                w.WriteLine("global::CommonBasicLibraries.CollectionClasses.BasicList<string> output = [];")
                .WriteLine("foreach (var item in _originalList)")
                .WriteCodeBlock(w =>
                {
                    foreach (var p in result.Properties)
                    {
                        w.WriteLine($"""
                            if (item.Key == "{p}")
                            """)
                        .WriteCodeBlock(w =>
                        {
                            w.WriteLine($"if (global::CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions.InterfaceExtensions.IsUpdate(this, item.Value, {p}))")
                            .WriteCodeBlock(w => w.WriteLine("output.Add(item.Key);"));
                        });
                    }
                })
                .WriteLine("return output;");
            });
        return w;
    }
    public static ICodeBlock PopulateMiddle(this ICodeBlock w)
    {
        w.WriteLine("private Dictionary<string, object?> _originalList = [];")
            .WriteLine("public void Initialize()")
            .WriteCodeBlock(w => w.WriteLine("_originalList = GetCurrentValues();"));
        return w;
    }
    public static ICodeBlock PopulateGetCurrentValuesMethod(this ICodeBlock w, ResultsModel result)
    {
        w.WriteLine("private Dictionary<string, object?> GetCurrentValues()")
            .WriteCodeBlock(w =>
            {
                w.WriteLine("Dictionary<string, object?> output = [];");
                foreach (var p in result.Properties)
                {
                    w.WriteLine($"""
                output.Add("{p}", TagNumber);
                """);
                }
                w.WriteLine("return output;");
            });

        return w;
    }
}