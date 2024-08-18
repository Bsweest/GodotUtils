using GodotUtils.InstanceResolver.SourceGenerators.Extensions;
using Microsoft.CodeAnalysis;

namespace GodotUtils.InstanceResolver.SourceGenerators;

partial class ParameterGenerators
{
    private static class Validator
    {
        public static bool IsTargetTypeValid(IFieldSymbol fieldSymbol)
        {
            return fieldSymbol.ContainingType.ImplementsFromFullyQualifiedMetadataName(
                    "GodotUtils.InstanceResolver.IResolvableNode"
                )
                && fieldSymbol.ContainingType.InheritsFromFullyQualifiedMetadataName("Godot.Node");
        }
    }
}
