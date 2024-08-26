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
        IncrementalValuesProvider<(
            HierarchyInfo Hierarchy,
            Result<ImmutableArray<PropertyInfo>> Info
        )> classInfoWithErrors = context.SyntaxProvider.ForAttributeWithMetadataName(
            "GodotUtils.InstanceResolver.ResolvableNodeAttribute",
            static (node, _) => node is ClassDeclarationSyntax,
            static (atx, token) =>
            {
                if (
                    !atx.SemanticModel.Compilation.HasLanguageVersionAtLeastEqualTo(
                        LanguageVersion.CSharp8
                    )
                )
                    return default!;

                INamedTypeSymbol typeSymbol = (INamedTypeSymbol)atx.TargetSymbol;
                Execute.TryGetClassInfo(typeSymbol, out var diagnostics);

                var fields = typeSymbol
                    .GetMembers()
                    .Where(member =>
                        member is IFieldSymbol field
                        && field.HasAttributeWithFullyQualifiedMetadataName(
                            "GodotUtils.InstanceResolver.ParameterAttribute"
                        )
                    )
                    .Cast<IFieldSymbol>();

                token.ThrowIfCancellationRequested();

                using var builder = ImmutableArrayBuilder<PropertyInfo>.Rent();

                foreach (var field in fields)
                {
                    Execute.TryGetFieldInfo(field, token, out PropertyInfo? propertyInfo);

                    if (propertyInfo != null)
                        builder.Add(propertyInfo);

                    token.ThrowIfCancellationRequested();
                }

                return (
                    HierarchyInfo.From(typeSymbol),
                    new Result<ImmutableArray<PropertyInfo>>(builder.ToImmutable(), diagnostics)
                );
            }
        );

        context.ReportDiagnostics(classInfoWithErrors.Select(static (item, _) => item.Info.Errors));
        var classInfo = classInfoWithErrors.Where(static item => item.Info.Errors.IsEmpty)!;

        context.RegisterSourceOutput(
            classInfo,
            static (context, item) =>
            {
                // Generate all member declarations for the current type
                ImmutableArray<MemberDeclarationSyntax> memberDeclarations = item
                    .Info.Value.SelectMany(Execute.GetPropertySyntax)
                    .ToImmutableArray();

                ImmutableArray<StatementSyntax> expressionStatementSyntaxes = item
                    .Info.Value.Select(Execute.GetMapExpressionsSyntax)
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
