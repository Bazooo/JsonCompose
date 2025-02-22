using System.Collections.Generic;
using System.Linq;

namespace JsonCompose;

public static class Template
{
    public static string Composition(CompositionTemplateData data) =>
$$"""
#nullable enable
{{string.Join("\n", data.Usings)}}

namespace {{data.ClassNamespace}} 
{
    [JsonConverter(typeof({{data.ClassName}}Converter))]
    public partial class {{data.ClassName}};

    file class {{data.ClassName}}Converter : JsonConverter<{{data.ClassName}}>
    {
        public override {{data.ClassName}} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            {{string.Join("\n            ", data.ComponentProperties.Select((_, i) => $"var cpr{i} = reader;"))}}
            var root = JsonSerializer.Deserialize<RootObject>(ref reader, options);
            
            if (root == null)
                throw new JsonException($"Unable to deserialize '{{data.ClassName}}'");
            
            return new {{data.ClassName}}
            {
                {{string.Join("\n                ", data.ComponentProperties.Select((cp, i) => $"{cp.Name} = JsonSerializer.Deserialize<{cp.Type}>(ref cpr{i}, options)!,"))}}
                {{string.Join("\n                ", data.RootPropertySyntaxes.Select(rp => $"{rp.PropertyName} = root.{rp.PropertyName},"))}}
            };
        }

        public override void Write(Utf8JsonWriter writer, {{data.ClassName}} value, JsonSerializerOptions options)
        {
            var rootElement = JsonSerializer.SerializeToElement(new RootObject
            {
                {{string.Join("\n                ", data.RootPropertySyntaxes.Select(rp => $"{rp.PropertyName} = value.{rp.PropertyName},"))}}
            }, options);
            var componentElements = new List<JsonElement>()
            {
                {{string.Join("\n                ", data.ComponentProperties.Select(cp => $"JsonSerializer.SerializeToElement(value.{cp.Name}, options),"))}}
            };
        
            writer.WriteStartObject();
            foreach (var rootProperty in rootElement.EnumerateObject())
            {
                rootProperty.WriteTo(writer);
            }
            
            foreach (var componentProperty in componentElements.SelectMany(e => e.EnumerateObject()))
            {
                componentProperty.WriteTo(writer);
            }
            writer.WriteEndObject();
        }
    }
    
    file class RootObject
    {
        {{string.Join("\n        ", data.RootPropertySyntaxes.SelectMany(rp => rp.Syntax.Split('\n')).Select(s => s.Trim()))}}
    } 
}
""";

    public class CompositionTemplateData
    {
        public required IEnumerable<string> Usings { get; init; }
        
        public required string ClassNamespace { get; init; }
        
        public required string ClassName { get; init; }
        
        public required IEnumerable<PropertySyntax> RootPropertySyntaxes { get; init; }
        
        public required IEnumerable<Property> ComponentProperties { get; init; }

        public sealed record Property(string Name, string Type);

        public sealed record PropertySyntax(string PropertyName, string Syntax);
    }
}