using GodotUtils.InstanceResolver.Generators;
using Microsoft.CodeAnalysis;

namespace GodotUtils.InstanceResolver.Generator.Diagnostics;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor InvalidContainingTypeForParameterFieldError =
        new(
            id: "GDIR0001",
            title: "Invalid containing type for [Parameter] field",
            messageFormat: "The field {0}.{1} cannot be used to generate an build parameter property, as its containing type doesn't inherit from IParamsResolvedNode, nor does it use [IParamsResolvedNode]",
            category: nameof(ParameterGenerators),
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Fields annotated with [Parameter] must be contained in a type that inherits from IParamsResolvedNode or that is annotated with [IParamsResolvedNode] (including base types).",
            helpLinkUri: ""
        );
}
