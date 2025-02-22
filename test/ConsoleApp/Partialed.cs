using System.Text.Json.Serialization;
using JsonCompose;

namespace ConsoleApp;

public partial class ClassModel
{
    [JsonIgnore]
    public string? Color { get; init; }
    
    [Component]
    public required Address Address { get; init; }
}

[JsonCompose]
public partial class Address
{
    [JsonIgnore]
    public string? Street { get; init; }
    
    public required string City { get; init; }
    
    public required string Province { get; init; }
    
    [Component]
    public required ZipCode ZipCode { get; init; }
}

public class ZipCode
{
    public required string First { get; init; }
    
    public required string Last { get; init; }
}