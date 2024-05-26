using GodotUtils.InstanceResolver.Generators.Components;
using GodotUtils.InstanceResolver.Generators.Helper;
using Microsoft.CodeAnalysis;

namespace GodotUtils.InstanceResolver.Generators.Extensions;

internal static class DiagnosticExtensions
{
    public static void Add(
        this in ImmutableArrayBuilder<DiagnosticInfo> diagnostics,
        DiagnosticDescriptor descriptor,
        ISymbol symbol,
        params object[] args
    )
    {
        diagnostics.Add(DiagnosticInfo.Create(descriptor, symbol, args));
    }
}
