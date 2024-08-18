using System.Collections.Immutable;
using System.Linq;
using GodotUtils.InstanceResolver.SourceGenerators.Components;
using GodotUtils.InstanceResolver.SourceGenerators.Extensions;
using GodotUtils.InstanceResolver.SourceGenerators.Helper;
using GodotUtils.InstanceResolver.SourceGenerators.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GodotUtils.InstanceResolver.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public partial class ResolvableNodeGenerators : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        if (!System.Diagnostics.Debugger.IsAttached)
            System.Diagnostics.Debugger.Launch();

        IncrementalValuesProvider<(
            HierarchyInfo Hierarchy,
            Result<PropertyInfo?> Info
        )> propertyInfoWithErrors = context.SyntaxProvider.ForAttributeWithMetadataName(
            "GodotUtils.InstanceResolver.ResolvableNodeAttribute",
            static (node, _) => node is ClassDeclarationSyntax,
            static (attrContext, token) =>
            {
                if (
                    !attrContext.SemanticModel.Compilation.HasLanguageVersionAtLeastEqualTo(
                        LanguageVersion.CSharp8
                    )
                )
                    return default;

                INamedTypeSymbol typeSymbol = (INamedTypeSymbol)attrContext.TargetSymbol;
                HierarchyInfo hierarchyInfo = HierarchyInfo.From(typeSymbol);

                token.ThrowIfCancellationRequested();
            }
        );
    }

    public void Initialize_Prev(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<(
            HierarchyInfo Hierarchy,
            Result<PropertyInfo?> Info
        )> propertyInfoWithErrors = context
            .SyntaxProvider.ForAttributeWithMetadataName(
                "GodotUtils.InstanceResolver.ParameterAttribute",
                static (node, _) =>
                    node
                        is VariableDeclaratorSyntax
                        {
                            Parent: VariableDeclarationSyntax
                            {
                                Parent: FieldDeclarationSyntax { Parent: ClassDeclarationSyntax }
                            }
                        },
                static (context, token) =>
                {
                    if (
                        !context.SemanticModel.Compilation.HasLanguageVersionAtLeastEqualTo(
                            LanguageVersion.CSharp8
                        )
                    )
                        return default;

                    IFieldSymbol fieldSymbol = (IFieldSymbol)context.TargetSymbol;

                    // Get the hierarchy info for the target symbol, and try to gather the property info
                    HierarchyInfo hierarchy = HierarchyInfo.From(fieldSymbol.ContainingType);

                    token.ThrowIfCancellationRequested();

                    _ = Execute.TryGetInfo(
                        context.Attributes[0],
                        fieldSymbol,
                        token,
                        out PropertyInfo? propertyInfo,
                        out ImmutableArray<DiagnosticInfo> diagnostics
                    );

                    token.ThrowIfCancellationRequested();

                    return (
                        Hierarchy: hierarchy,
                        Info: new Result<PropertyInfo?>(propertyInfo, diagnostics)
                    );
                }
            )
            .Where(static item => item.Hierarchy is not null);

        context.ReportDiagnostics(
            propertyInfoWithErrors.Select(static (item, _) => item.Info.Errors)
        );

        IncrementalValuesProvider<(
            HierarchyInfo Hierarchy,
            Result<PropertyInfo> Info
        )> propertyInfo = propertyInfoWithErrors.Where(static item => item.Info.Value is not null)!;

        IncrementalValuesProvider<(
            HierarchyInfo Hierarchy,
            EquatableArray<PropertyInfo> Properties
        )> groupedPropertyInfo = propertyInfo.GroupBy(
            static item => item.Left,
            static item => item.Right.Value
        );

        context.RegisterSourceOutput(
            groupedPropertyInfo,
            static (context, item) =>
            {
                // Generate all member declarations for the current type
                ImmutableArray<MemberDeclarationSyntax> memberDeclarations = item
                    .Properties.SelectMany(Execute.GetPropertySyntax)
                    .ToImmutableArray();

                ImmutableArray<StatementSyntax> expressionStatementSyntaxes = item
                    .Properties.Select(Execute.GetMapExpressionsSyntax)
                    .ToImmutableArray();

                // Insert all members into the same partial type declaration
                CompilationUnitSyntax compilationUnit = item.Hierarchy.GetCompilationUnit(
                    memberDeclarations,
                    expressionStatementSyntaxes
                );

                context.AddSource($"{item.Hierarchy.FilenameHint}.g.cs", compilationUnit);
            }
        );
    }
}
