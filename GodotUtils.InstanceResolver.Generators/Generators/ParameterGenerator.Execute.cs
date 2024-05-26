using GodotUtils.InstanceResolver.Generators.Components;
using GodotUtils.InstanceResolver.Generators.Diagnostics;
using GodotUtils.InstanceResolver.Generators.Extensions;
using GodotUtils.InstanceResolver.Generators.Helper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Globalization;
using System.Threading;
using static GodotUtils.InstanceResolver.Generators.Constants.ClassNameConst;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GodotUtils.InstanceResolver.Generators;

partial class ParameterGenerators
{
    private static class Execute
    {
        public static bool TryGetInfo(
            AttributeData attributeData,
            IFieldSymbol fieldSymbol,
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

            bool isRequired = true;
            foreach (bool? dependentIsRequired in attributeData.GetConstructorArguments<bool>())
            {
                if (dependentIsRequired is bool boolValue)
                {
                    isRequired = boolValue;
                    break;
                }
            }

            propertyInfo = new PropertyInfo(
                typeNameWithNullabilityAnnotations,
                fieldName,
                propertyName,
                new(isRequired)
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

            SyntaxToken[] modifierTokens = [Token(SyntaxKind.PublicKeyword)];

            if (propertyInfo.AttributeInfo.IsRequired)
                modifierTokens = [.. modifierTokens, Token(SyntaxKind.RequiredKeyword)];

            return PropertyDeclaration(propertyType, Identifier(propertyInfo.PropertyName))
                .AddModifiers(modifierTokens)
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
