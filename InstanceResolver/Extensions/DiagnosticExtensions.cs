using GodotUtils.InstanceResolver.Generator.Components;
using GodotUtils.InstanceResolver.Generator.Helper;
using Microsoft.CodeAnalysis;

namespace GodotUtils.InstanceResolver.Generator.Extensions;

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
