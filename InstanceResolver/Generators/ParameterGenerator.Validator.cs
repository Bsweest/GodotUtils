using GodotUtils.InstanceResolver.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace GodotUtils.InstanceResolver.Generators;

partial class ParameterGenerators
{
    private static class Validator
    {
        public static bool IsTargetTypeValid(IFieldSymbol fieldSymbol)
        {
            return fieldSymbol.ContainingType.ImplementsFromFullyQualifiedMetadataName(
                "GodotUtils.InstanceResolver.IParamsResolveNode"
            ) && fieldSymbol.ContainingType.InheritsFromFullyQualifiedMetadataName("Godot.Node");
        }
    }
}
