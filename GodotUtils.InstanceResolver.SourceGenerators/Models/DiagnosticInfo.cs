using System.Collections.Immutable;
using System.Linq;
using GodotUtils.InstanceResolver.SourceGenerators.Helper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GodotUtils.InstanceResolver.SourceGenerators.Models;

internal sealed record DiagnosticInfo(
    DiagnosticDescriptor Descriptor,
    SyntaxTree? SyntaxTree,
    TextSpan TextSpan,
    EquatableArray<string> Arguments
)
{
    /// <summary>
    /// Creates a new <see cref="Diagnostic"/> instance with the state from this model.
    /// </summary>
    /// <returns>A new <see cref="Diagnostic"/> instance with the state from this model.</returns>
    public Diagnostic ToDiagnostic()
    {
        if (SyntaxTree is not null)
        {
            return Diagnostic.Create(
                Descriptor,
                Location.Create(SyntaxTree, TextSpan),
                Arguments.ToArray()
            );
        }

        return Diagnostic.Create(Descriptor, null, Arguments.ToArray());
    }

    /// <summary>
    /// Creates a new <see cref="DiagnosticInfo"/> instance with the specified parameters.
    /// </summary>
    /// <param name="descriptor">The input <see cref="DiagnosticDescriptor"/> for the diagnostics to create.</param>
    /// <param name="symbol">The source <see cref="ISymbol"/> to attach the diagnostics to.</param>
    /// <param name="args">The optional arguments for the formatted message to include.</param>
    /// <returns>A new <see cref="DiagnosticInfo"/> instance with the specified parameters.</returns>
    public static DiagnosticInfo Create(
        DiagnosticDescriptor descriptor,
        ISymbol symbol,
        params object[] args
    )
    {
        Location location = symbol.Locations.First();

        return new(
            descriptor,
            location.SourceTree,
            location.SourceSpan,
            args.Select(static arg => arg.ToString()).ToImmutableArray()!
        );
    }

    /// <summary>
    /// Creates a new <see cref="DiagnosticInfo"/> instance with the specified parameters.
    /// </summary>
    /// <param name="descriptor">The input <see cref="DiagnosticDescriptor"/> for the diagnostics to create.</param>
    /// <param name="node">The source <see cref="SyntaxNode"/> to attach the diagnostics to.</param>
    /// <param name="args">The optional arguments for the formatted message to include.</param>
    /// <returns>A new <see cref="DiagnosticInfo"/> instance with the specified parameters.</returns>
    public static DiagnosticInfo Create(
        DiagnosticDescriptor descriptor,
        SyntaxNode node,
        params object[] args
    )
    {
        Location location = node.GetLocation();

        return new(
            descriptor,
            location.SourceTree,
            location.SourceSpan,
            args.Select(static arg => arg.ToString()).ToImmutableArray()!
        );
    }
}
