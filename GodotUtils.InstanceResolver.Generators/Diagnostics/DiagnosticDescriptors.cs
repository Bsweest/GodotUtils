using Microsoft.CodeAnalysis;

namespace GodotUtils.InstanceResolver.Generators.Diagnostics;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor InvalidContainingTypeForParameterFieldError =
        new(
            id: "GDIR0001",
            title: "Invalid containing type for [Parameter] field",
            messageFormat: "The field {0}.{1} cannot be used to generate an build parameter property, as its containing type must inherit from Godot Node and use [IParamsResolvedNode]",
            category: nameof(ParameterGenerators),
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Fields annotated with [Parameter] must be contained in a type that inherits Godot Node and implements [IParamsResolvedNode].",
            helpLinkUri: ""
        );
}
