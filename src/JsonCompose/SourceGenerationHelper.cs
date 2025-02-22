namespace JsonCompose;

public static class SourceGenerationHelper
{
    public const string Attribute =
"""
namespace JsonCompose
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class JsonComposeAttribute : System.Attribute;
    
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ComponentAttribute : System.Attribute;
}
""";
}