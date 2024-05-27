using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotUtils.InstanceResolver.Generators.Components;

internal sealed record ParameterInfo(EqualsValueClauseSyntax? Value);
