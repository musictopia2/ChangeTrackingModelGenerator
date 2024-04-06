namespace ChangeTrackingModelGenerator;
public class ParserClass(IEnumerable<ClassDeclarationSyntax> list, Compilation compilation)
{
    public BasicList<ResultsModel> GetResults()
    {
        BasicList<ResultsModel> output = [];
        foreach (var item in list)
        {
            ResultsModel results = GetResult(item);
            //class names cannot be repeating.
            if (output.Any(x => x.ClassName == results.ClassName) == false)
            {
                output.Add(results);
            }
        }
        return output;
    }
    private ResultsModel GetResult(ClassDeclarationSyntax classDeclaration)
    {
        ResultsModel output = new();
        SemanticModel compilationSemanticModel = classDeclaration.GetSemanticModel(compilation);
        INamedTypeSymbol symbol = compilationSemanticModel.GetDeclaredSymbol(classDeclaration)!;
        output.ClassName = symbol.Name;
        if (symbol.Implements("ISimpleDatabaseEntity"))
        {
            output.InterfaceCategory = EnumInterfaceCategory.Database;
        }
        else
        {
            output.InterfaceCategory = EnumInterfaceCategory.None;
        }
        output.Namespace = $"{symbol.ContainingNamespace.ToDisplayString()}"; //hopefully okay now (?)
        //eventually will go further.
        output.Properties = GetProperties(symbol);
        return output;
    }
    private BasicList<string> GetProperties(INamedTypeSymbol classSymbol)
    {
        BasicList<string> output = [];
        var firsts = classSymbol.GetAllPublicProperties();
        foreach (var item in firsts)
        {
            if (item.HasAttribute("NotMapped") == true || item.HasAttribute("ExcludeUpdateListener") == true || item.Name.ToLower() == "id")
            {
                continue;
            }
            INamedTypeSymbol other;
            if (item.Type.Name == "Nullable")
            {
                //output.Nullable = true;
                INamedTypeSymbol temp;
                temp = (INamedTypeSymbol)item.Type;
                other = (INamedTypeSymbol)temp.TypeArguments[0];
            }
            else
            {
                other = (INamedTypeSymbol)item.Type;
            }
            if (IsSimpleType(other) == false)
            {
                continue;
            }
            output.Add(item.Name);
        }
        return output;
    }
    private bool IsSimpleType(INamedTypeSymbol symbol)
    {
        if (symbol.TypeKind == TypeKind.Enum)
        {
            return true;
        }
        else if (symbol.Name.StartsWith("Enum"))
        {
            return true;
        }
        else if (symbol.Name == "Int32")
        {
            return true;
        }
        else if (symbol.Name == "Boolean")
        {
            return true;
        }
        else if (symbol.Name == "Decimal")
        {
            return true;
        }
        else if (symbol.Name == "Double")
        {
            return true;
        }
        else if (symbol.Name == "Float" || symbol.Name == "Single")
        {
            return true;
        }
        else if (symbol.Name == "DateOnly")
        {
            return true;
        }
        else if (symbol.Name == "DateTime")
        {
            return true;
        }
        else if (symbol.Name == "String")
        {
            return true;
        }
        else if (symbol.Name == "Char")
        {
            return true;
        }
        else
        {
            return false;
            //most likely, if none of those, then 
        }
    }
}