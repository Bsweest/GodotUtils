using GodotUtils.InstanceResolver.SourceGenerators.Helper;
using GodotUtils.InstanceResolver.SourceGenerators.Models;
using Microsoft.CodeAnalysis;

namespace GodotUtils.InstanceResolver.SourceGenerators.Extensions;

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
