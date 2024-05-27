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

            var paramInfo = GetParameterInfo(fieldSymbol);

            token.ThrowIfCancellationRequested();

            propertyInfo = new PropertyInfo(
                typeNameWithNullabilityAnnotations,
                fieldName,
                propertyName,
                paramInfo
            );
            diagnostics = builder.ToImmutable();

            return true;
        }

        private static string GetGeneratedPropertyName(IFieldSymbol fieldSymbol)
        {
            string propertyName = fieldSymbol.Name;
            if (propertyName.StartsWith('_'))
            {
                propertyName = propertyName.TrimStart('_');
            }

            return $"{char.ToUpper(propertyName[0], CultureInfo.InvariantCulture)}{propertyName[1..]}";
        }

        private static ParameterInfo GetParameterInfo(IFieldSymbol fieldSymbol)
        {
            var equalSyntax = fieldSymbol.DeclaringSyntaxReferences[0].GetSyntax() switch
            {
                PropertyDeclarationSyntax property => property.Initializer,
                VariableDeclaratorSyntax variable => variable.Initializer,
                _ => null,
            };

            return new(equalSyntax);
        }

        public static MemberDeclarationSyntax GetPropertySyntax(PropertyInfo propertyInfo)
        {
            TypeSyntax propertyType = IdentifierName(
                propertyInfo.TypeNameWithNullabilityAnnotations
            );

            var declaration = PropertyDeclaration(propertyType, Identifier(propertyInfo.PropertyName))
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                    AccessorDeclaration(SyntaxKind.InitAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                );

            if (propertyInfo.AttributeInfo.Value is EqualsValueClauseSyntax defaultValue)
            {
                declaration = declaration.WithInitializer(defaultValue).WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            }
            else
                declaration = declaration.AddModifiers(Token(SyntaxKind.RequiredKeyword));

            return declaration;
        }

        public static ExpressionStatementSyntax GetMapExpressionsSyntax(PropertyInfo info)
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
