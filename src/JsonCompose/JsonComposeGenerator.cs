using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace JsonCompose;

[Generator]
public class JsonComposeGenerator : IIncrementalGenerator
{
    private static readonly string[] Usings =
    [
        "using System.Text.Json;",
        "using System.Text.Json.Serialization;",
    ];

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "JsonComposeGenerator.g.cs",
            SourceText.From(SourceGenerationHelper.Attribute, Encoding.UTF8)));
        
        var declarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                transform: static (ctx, _) => GetTargetForGeneration(ctx));
        
        var compilationAndClasses = context.CompilationProvider.Combine(declarations.Collect());
        
        context.RegisterSourceOutput(compilationAndClasses, (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode) =>
        syntaxNode is ClassDeclarationSyntax {AttributeLists.Count: > 0} classDeclarationSyntax
            && classDeclarationSyntax.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString().Equals("JsonCompose", StringComparison.Ordinal)));

    private static ClassDeclarationSyntax GetTargetForGeneration(GeneratorSyntaxContext context) => (ClassDeclarationSyntax)context.Node;

    private static void Execute(Compilation compilation, 
        ImmutableArray<ClassDeclarationSyntax> classes, 
        SourceProductionContext context)
    {
        foreach (var classSyntax in classes)
        {
            var model = compilation.GetSemanticModel(classSyntax.SyntaxTree);

            if (model.GetDeclaredSymbol(classSyntax) is not { } classSymbol)
                continue;
            
            if (!classSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
            {
                var error = Diagnostic.Create(DiagnosticsDescriptors.ClassIsNotPartialMessage,
                    classSyntax.Identifier.GetLocation(),
                    classSymbol.Name);
                context.ReportDiagnostic(error);
                return;
            }

            var classMembers = classSymbol.GetMembers();
            
            var classProperties = classMembers
                .OfType<IPropertySymbol>()
                .Where(s => s.DeclaredAccessibility > Accessibility.Private)
                .ToList();

            var componentProperties = classProperties.Where(p => p.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString().Equals("JsonCompose.ComponentAttribute") ?? false)).ToList();
            var wrongTypeComponentProperties = componentProperties.Where(cp => cp.Type.TypeKind is not TypeKind.Class).ToList();

            foreach (var componentProperty in wrongTypeComponentProperties)
            {
                var error = Diagnostic.Create(DiagnosticsDescriptors.ComponentIsWrongTypeMessage,
                    componentProperty.DeclaringSyntaxReferences.First().GetSyntax().GetLocation(),
                    componentProperty.Name);
                context.ReportDiagnostic(error);
            }

            if (wrongTypeComponentProperties.Any())
                return;
            
            var rootPropertySyntaxes = classProperties
                .Where(p => !componentProperties.Contains(p))
                .SelectMany(cp => cp.DeclaringSyntaxReferences.Select(x => x.GetSyntax() as PropertyDeclarationSyntax))
                .WhereNotNull()
                .ToList();
            
            var classUsings = classSymbol.DeclaringSyntaxReferences.SelectMany(sr => sr.SyntaxTree.GetRoot().ChildNodes().OfType<UsingDirectiveSyntax>());

            var sourceCode = Template.Composition(new Template.CompositionTemplateData
            {
                Usings = classUsings.Select(ud => ud.ToFullString().TrimEnd()).Concat(Usings).ToImmutableHashSet(),
                ClassName = classSymbol.Name,
                ClassNamespace = classSymbol.ContainingNamespace.ToDisplayString(),
                RootPropertySyntaxes = rootPropertySyntaxes.Select(pd => new Template.CompositionTemplateData.PropertySyntax(pd.Identifier.ToString(), pd.ToString())),
                ComponentProperties = componentProperties.Select(cp => new Template.CompositionTemplateData.Property(cp.Name, cp.Type.ToDisplayString())),
            });

            context.AddSource(
                $"{classSymbol.ContainingNamespace}.{classSymbol.Name}.Composition.g.cs",
                SourceText.From(sourceCode, Encoding.UTF8)
            );
        }
    }
}