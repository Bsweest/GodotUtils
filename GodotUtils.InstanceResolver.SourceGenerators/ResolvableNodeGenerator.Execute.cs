using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using GodotUtils.InstanceResolver.SourceGenerators.Components;
using GodotUtils.InstanceResolver.SourceGenerators.Diagnostics;
using GodotUtils.InstanceResolver.SourceGenerators.Extensions;
using GodotUtils.InstanceResolver.SourceGenerators.Helper;
using GodotUtils.InstanceResolver.SourceGenerators.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static GodotUtils.InstanceResolver.SourceGenerators.Constants.ClassNameConst;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GodotUtils.InstanceResolver.SourceGenerators;

partial class ResolvableNodeGenerators
{
    private static class Execute
    {
        public static void TryGetFieldInfo(
            IFieldSymbol fieldSymbol,
            CancellationToken token,
            [NotNullWhen(true)] out PropertyInfo? propertyInfo
        )
        {
            string typeNameWithNullabilityAnnotations =
                fieldSymbol.Type.GetFullyQualifiedNameWithNullabilityAnnotations();
            string fieldName = fieldSymbol.Name;
            string propertyName = GetGeneratedPropertyName(fieldSymbol);

            var paramInfo = GetParameterInfo(fieldSymbol);

            token.ThrowIfCancellationRequested();

            propertyInfo = new PropertyInfo(
                typeNameWithNullabilityAnnotations,
                fieldName,
                $"{fieldName}Wrapper",
                propertyName,
                paramInfo
            );
        }

        public static void TryGetClassInfo(
            ITypeSymbol typeSymbol,
            out ImmutableArray<DiagnosticInfo> diagnostics
        )
        {
            using var builder = ImmutableArrayBuilder<DiagnosticInfo>.Rent();

            if (!typeSymbol.InheritsFromFullyQualifiedMetadataName("Godot.Node"))
            {
                builder.Add(
                    DiagnosticDescriptors.InvalidTypeForResolvableNodeError,
                    typeSymbol,
                    typeSymbol.Name
                );

                diagnostics = builder.ToImmutable();
                return;
            }

            diagnostics = builder.ToImmutable();
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

            return new(equalSyntax == null);
        }

        public static MemberDeclarationSyntax[] GetPropertySyntax(PropertyInfo propertyInfo)
        {
            TypeSyntax propertyType = IdentifierName(
                propertyInfo.TypeNameWithNullabilityAnnotations
            );
            var wrapperName = IdentifierName(propertyInfo.WrapperName);

            MemberDeclarationSyntax[] members = [];

            var getter = AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            var initer = AccessorDeclaration(SyntaxKind.InitAccessorDeclaration);

            var declaration = PropertyDeclaration(
                    propertyType,
                    Identifier(propertyInfo.PropertyName)
                )
                .AddModifiers(Token(SyntaxKind.PublicKeyword));

            if (propertyInfo.AttributeInfo.IsRequired)
            {
                declaration = declaration.AddModifiers(Token(SyntaxKind.RequiredKeyword));
                initer = initer.WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                members =
                [
                    FieldDeclaration(
                            VariableDeclaration(
                                    IdentifierName(
                                        OptionalValueField(
                                            propertyInfo.TypeNameWithNullabilityAnnotations
                                        )
                                    )
                                )
                                .AddVariables(
                                    VariableDeclarator(Identifier(propertyInfo.WrapperName))
                                        .WithInitializer(
                                            EqualsValueClause(ImplicitObjectCreationExpression())
                                        )
                                )
                        )
                        .AddModifiers(
                            Token(SyntaxKind.PrivateKeyword),
                            Token(SyntaxKind.ReadOnlyKeyword)
                        ),
                    MethodDeclaration(
                            PredefinedType(Token(SyntaxKind.BoolKeyword)),
                            Identifier($"Is{propertyInfo.PropertyName}Initialized")
                        )
                        .AddModifiers(Token(SyntaxKind.PublicKeyword))
                        .WithExpressionBody(
                            ArrowExpressionClause(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    wrapperName,
                                    IdentifierName(BuildFunctionConst.IsInitialized)
                                )
                            )
                        )
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                ];

                getter = getter.WithExpressionBody(
                    ArrowExpressionClause(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            wrapperName,
                            IdentifierName(BuildFunctionConst.GetValue)
                        )
                    )
                );

                initer = initer.AddBodyStatements(
                    ExpressionStatement(
                        InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    wrapperName,
                                    IdentifierName(BuildFunctionConst.SetValue)
                                )
                            )
                            .AddArgumentListArguments(
                                Argument(IdentifierName(BuildFunctionConst.ValueKeyword))
                            )
                    )
                );
            }

            declaration = declaration.AddAccessorListAccessors(getter, initer);

            members = [.. members, declaration];

            return members;
        }

        public static StatementSyntax GetMapExpressionsSyntax(PropertyInfo info)
        {
            StatementSyntax expression = ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(info.FieldName),
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(BuildFunctionConst.PassingObj),
                        IdentifierName(info.PropertyName)
                    )
                )
            );

            if (!info.AttributeInfo.IsRequired)
            {
                expression = IfStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(BuildFunctionConst.PassingObj),
                            IdentifierName($"Is{info.PropertyName}Initialized")
                        )
                    ),
                    Block(expression)
                );
            }

            return expression;
        }
    }
}
