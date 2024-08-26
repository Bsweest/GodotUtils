using Microsoft.CodeAnalysis;

namespace GodotUtils.InstanceResolver.SourceGenerators.Diagnostics;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor InvalidTypeForResolvableNodeError =
        new(
            id: "GDIR0001",
            title: "Invalid type for [ResolvableNode] attribute",
            messageFormat: "The class {0} cannot be used to generate resolvable node class, as its type must inherit from Godot.Node",
            category: nameof(ResolvableNodeGenerators),
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Class annotated with [ResolvableNode] must inherit from Godot.Node.",
            helpLinkUri: ""
        );
}
