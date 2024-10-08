using GodotUtils.InstanceResolver.SourceGenerators.Extensions;
using GodotUtils.InstanceResolver.SourceGenerators.Helper;
using GodotUtils.InstanceResolver.SourceGenerators.Models;
using Microsoft.CodeAnalysis;
using static Microsoft.CodeAnalysis.SymbolDisplayTypeQualificationStyle;

namespace GodotUtils.InstanceResolver.SourceGenerators.Components;

internal sealed partial record HierarchyInfo(
    string FilenameHint,
    string MetadataName,
    string Namespace,
    EquatableArray<TypeInfor> Hierarchy
)
{
    /// <summary>
    /// Creates a new <see cref="HierarchyInfo"/> instance from a given <see cref="INamedTypeSymbol"/>.
    /// </summary>
    /// <param name="typeSymbol">The input <see cref="INamedTypeSymbol"/> instance to gather info for.</param>
    /// <returns>A <see cref="HierarchyInfo"/> instance describing <paramref name="typeSymbol"/>.</returns>
    public static HierarchyInfo From(INamedTypeSymbol typeSymbol)
    {
        using var hierarchy = ImmutableArrayBuilder<TypeInfor>.Rent();

        for (
            INamedTypeSymbol? parent = typeSymbol;
            parent is not null;
            parent = parent.ContainingType
        )
        {
            hierarchy.Add(
                new TypeInfor(
                    parent.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                    parent.TypeKind,
                    parent.IsRecord
                )
            );
        }

        return new(
            typeSymbol.GetFullyQualifiedMetadataName(),
            typeSymbol.MetadataName,
            typeSymbol.ContainingNamespace.ToDisplayString(
                new(typeQualificationStyle: NameAndContainingTypesAndNamespaces)
            ),
            hierarchy.ToImmutable()
        );
    }
}
