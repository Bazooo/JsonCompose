using JsonCompose;

namespace ConsoleApp;

[JsonCompose]
public partial class ClassModel
{
    public required int Id { get; init; }

    [Component]
    public required Name Name { get; init; }
}

public class Name
{
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
}
