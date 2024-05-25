using GodotUtils.InstanceResolver.Generator.Components;
using GodotUtils.InstanceResolver.Generator.Diagnostics;
using GodotUtils.InstanceResolver.Generator.Extensions;
using GodotUtils.InstanceResolver.Generator.Helper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Globalization;
using System.Threading;
using static GodotUtils.InstanceResolver.Generator.Constants.ClassNameConst;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GodotUtils.InstanceResolver.Generators;

partial class ParameterGenerators
{
    private static class Execute
    {
        public static bool TryGetInfo(
            FieldDeclarationSyntax fieldSyntax,
            IFieldSymbol fieldSymbol,
            SemanticModel semanticModel,
            CancellationToken token,
            out PropertyInfo? propertyInfo,
            out ImmutableArray<DiagnosticInfo> diagnostics
        )
        {
            using var builder = ImmutableArrayBuilder<DiagnosticInfo>.Rent();

            if (!Validator.IsTargetTypeValid(fieldSymbol))
            {
                builder.Add(
                    DiagnosticDescriptors.InvalidContainingTypeForParameterFieldError,
                    fieldSymbol,
                    fieldSymbol.ContainingType,
                    fieldSymbol.Name
                );

                propertyInfo = null;
                diagnostics = builder.ToImmutable();

                return false;
            }

            token.ThrowIfCancellationRequested();

            string typeNameWithNullabilityAnnotations =
                fieldSymbol.Type.GetFullyQualifiedNameWithNullabilityAnnotations();
            string fieldName = fieldSymbol.Name;
            string propertyName = GetGeneratedPropertyName(fieldSymbol);

            propertyInfo = new PropertyInfo(
                typeNameWithNullabilityAnnotations,
                fieldName,
                propertyName
            );
            diagnostics = builder.ToImmutable();

            return true;
        }

        public static string GetGeneratedPropertyName(IFieldSymbol fieldSymbol)
        {
            string propertyName = fieldSymbol.Name;
            if (propertyName.StartsWith('_'))
            {
                propertyName = propertyName.TrimStart('_');
            }

            return $"{char.ToUpper(propertyName[0], CultureInfo.InvariantCulture)}{propertyName[1..]}";
        }

        public static MemberDeclarationSyntax GetPropertySyntax(PropertyInfo propertyInfo)
        {
            TypeSyntax propertyType = IdentifierName(
                propertyInfo.TypeNameWithNullabilityAnnotations
            );

            return PropertyDeclaration(propertyType, Identifier(propertyInfo.PropertyName))
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                    AccessorDeclaration(SyntaxKind.InitAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                );
        }

        internal static ExpressionStatementSyntax GetMapExpressionsSyntax(PropertyInfo info)
        {
            return ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(info.FieldName),
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(PassingObj),
                        IdentifierName(info.PropertyName)
                    )
                )
            );
        }
    }
}
