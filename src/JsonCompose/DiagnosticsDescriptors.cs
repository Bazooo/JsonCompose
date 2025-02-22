using Microsoft.CodeAnalysis;

namespace JsonCompose;

public static class DiagnosticsDescriptors
{
    public static readonly DiagnosticDescriptor ClassIsNotPartialMessage = new(
        id: "JSC001",
        title: "Class is not partial",
        messageFormat: "The class '{0}' is not partial, which is required by the JsonComposeAttribute",
        category: "Generator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    
    public static readonly DiagnosticDescriptor ComponentIsWrongTypeMessage = new(
        id: "JSC002",
        title: "Component is of wrong type",
        messageFormat: "The component '{0}' type is not supported, only classes are supported",
        category: "Generator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}