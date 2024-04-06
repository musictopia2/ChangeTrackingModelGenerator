namespace ChangeTrackingModelGenerator;
public record ResultsModel
{
    public string ClassName { get; set; } = "";
    public EnumInterfaceCategory InterfaceCategory { get; set; }
    public string Namespace { get; set; } = ""; //this way i can use that to create this partial class.
    public BasicList<string> Properties { get; set; } = [];
}