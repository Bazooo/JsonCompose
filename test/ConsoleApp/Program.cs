using System.Text.Json;
using ConsoleApp;

var classModel = new ClassModel
{
    Id = 300,
    Color = "Red",
    Address = new Address
    {
        Street = "123 Main St",
        City = "Toronto",
        Province = "Ontario",
        ZipCode = new ZipCode
        {
            First = "A1A",
            Last = "1A1",
        },
    },
    Name = new Name
    {
        FirstName = "Barry",
        LastName = "Benson",
    },
};

var serialized = JsonSerializer.Serialize(classModel, new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
});

Console.WriteLine(serialized);