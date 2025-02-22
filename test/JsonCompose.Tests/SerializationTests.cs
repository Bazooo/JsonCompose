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
    public Task Serialize()
    {
        var serialized = JsonSerializer.Serialize(BasePerson);
        return Verify(serialized);
    }
    
    [Test]
    public Task SerializeWithNamingPolicy()
    {
        var serialized = JsonSerializer.Serialize(BasePerson, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        });
        return Verify(serialized);
    }

    [Test]
    public Task SerializeThenDeserialize()
    {
        var serialized = JsonSerializer.Serialize(BasePerson);
        var deserialized = JsonSerializer.Deserialize<Person>(serialized);
        return Verify(deserialized);
    }
    
    [Test]
    public Task SerializeThenDeserializeWithNamingPolicy()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
        var serialized = JsonSerializer.Serialize(BasePerson, options);
        var deserialized = JsonSerializer.Deserialize<Person>(serialized, options);
        return Verify(deserialized);
    }
}