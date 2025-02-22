using System.Text.Json;
using JsonCompose.Tests.Models;

namespace JsonCompose.Tests;

public class SerializationTests
{
    private static readonly Person BasePerson = new()
    {
        Id = 1,
        FirstName = "John",
        LastName = "Doe",
        Birthday = new Birthday()
        {
            Day = 1,
            Month = 5,
            Year = 2000,
        },
        Employment = new Employment()
        {
            Status = EmploymentStatus.FullTime,
            MonthlySalary = 1000,
            CompanyName = "Test Company",
            JobTitle = "Software Engineer",
            Industry = "IT",
        },
    };
    
    [Test]
    public Task Basic()
    {
        var serialized = JsonSerializer.Serialize(BasePerson);
        return Verify(serialized);
    }
    
    [Test]
    public Task BasicWithNamingPolicy()
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
        var serialized = JsonSerializer.Serialize(BasePerson, jsonSerializerOptions);
        return Verify(serialized);
    }
}