namespace JsonCompose.Tests.Models;

[JsonCompose]
public partial class Person
{
    public required int Id { get; init; }
    
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    [Component]
    public required Birthday Birthday { get; init; }
    
    [Component]
    public required Employment Employment { get; init; }
}