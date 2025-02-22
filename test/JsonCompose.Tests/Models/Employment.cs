using System.Text.Json.Serialization;

namespace JsonCompose.Tests.Models;

public class Employment
{
    [JsonPropertyName("EmploymentStatus")]
    public required EmploymentStatus Status { get; init; }
    
    public required int MonthlySalary { get; init; }
    
    public required string JobTitle { get; init; }
    
    public required string CompanyName { get; init; }
    
    public required string Industry { get; init; }
}

public enum EmploymentStatus
{
    Unemployed,
    FullTime,
    PartTime,
    Contract,
    Freelance,
}