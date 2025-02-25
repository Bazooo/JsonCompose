using JsonCompose;

namespace ConsoleApp;

[JsonCompose]
public partial class ClassModel : Identity
{
    [Component]
    public required Name Name { get; init; }
}

public abstract class Identity
{
    public required int Id { get; init; }
}

public class Name
{
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
}
