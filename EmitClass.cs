namespace ChangeTrackingModelGenerator;
internal class EmitClass(ImmutableArray<ResultsModel> results, SourceProductionContext context)
{
    public void Emit()
    {
        foreach (var item in results)
        {
            WriteItem(item);
        }
    }

    //this means means that i can find much better ways of doing the values now.


    private void WriteItem(ResultsModel item)
    {
        SourceCodeStringBuilder builder = new();
        builder.StartChangeStub(w =>
        {
            w.PopulateGetChangesMethod(item)
            .PopulateMiddle()
            .PopulateGetCurrentValuesMethod(item);

        }, item);
        context.AddSource($"{item.ClassName}.ChangeTracking.g.cs", builder.ToString());
    }
}