using GodotUtils.InstanceResolver.SourceGenerators.Extensions;
using Microsoft.CodeAnalysis;

namespace GodotUtils.InstanceResolver.SourceGenerators;

partial class ResolvableNodeGenerators
{
    private static class Validator
    {
        public static bool IsTargetTypeValid(IFieldSymbol fieldSymbol)
        {
            return fieldSymbol.ContainingType.InheritsFromFullyQualifiedMetadataName("Godot.Node");
        }
    }
}
