namespace ChangeTrackingModelGenerator;
internal static class SourceBuilderExtensions
{
    public static void StartChangeStub(this SourceCodeStringBuilder builder, Action<ICodeBlock> action, ResultsModel result)
    {
        builder.WriteLine("#nullable enable")
        .WriteLine(w =>
        {
            w.Write("namespace ")
            .Write(result.Namespace)
            .Write(";");
        })
        .WriteLine(w =>
        {
            string toImplement;
            if (result.InterfaceCategory == EnumInterfaceCategory.Database)
            {
                toImplement = "IUpdatableEntity";
            }
            else
            {
                toImplement = "IChangeTracker";
            }
            w.Write("public partial class ")
            .Write(result.ClassName)
            .Write($" : {toImplement}");
        })
        .WriteCodeBlock(action.Invoke);
    }
}