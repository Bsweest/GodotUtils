using System.Linq.Expressions;
using GodotUtils.InstanceResolver.Extensions;
using static System.Linq.Expressions.Expression;

namespace GodotUtils.InstanceResolver.Models;

internal interface IStore { }

internal class Store<TNode> : IStore
{
    private readonly Type _nodeType;
    private readonly List<Action<TNode, IDependencyProvider>> injectionMethods = [];

    public Store()
    {
        _nodeType = typeof(TNode);
        RegisterMethod();
        RegisterFieldsAndProperties();
    }

    public TNode Inject(TNode node, IDependencyProvider provider)
    {
        injectionMethods.ForEach(mapAction => mapAction.Invoke(node, provider));
        return node;
    }

    private void RegisterMethod()
    {
        var method = _nodeType
            .GetMethods()
            .Where(method => method.HasInjectAttribute())
            .FirstOrDefault();

        if (method == null)
            return;

        var methodParamTypes = method.GetParameters().Select(param => param.ParameterType);

        var node = Parameter(typeof(TNode), "node");
        var provider = Parameter(typeof(IDependencyProvider), "provider");

        var expression = Lambda<Action<TNode, IDependencyProvider>>(
            Call(node, method, methodParamTypes.Select(param => GetServiceCall(provider, param))),
            node,
            provider
        );

        injectionMethods.Add(expression.Compile());
    }

    private void RegisterFieldsAndProperties()
    {
        var node = Parameter(typeof(TNode), "node");
        var provider = Parameter(typeof(IDependencyProvider), "provider");

        injectionMethods.AddRange(
            _nodeType
                .GetFields()
                .Where(field => field.HasInjectAttribute() && field.IsPublic)
                .Select(fieldInfo =>
                    Lambda<Action<TNode, IDependencyProvider>>(
                            Assign(
                                Field(node, fieldInfo.Name),
                                GetServiceCall(provider, fieldInfo.FieldType)
                            ),
                            node,
                            provider
                        )
                        .Compile()
                )
        );

        injectionMethods.AddRange(
            _nodeType
                .GetProperties()
                .Where(property => property.HasInjectAttribute())
                .Select(methodInfo =>
                    Lambda<Action<TNode, IDependencyProvider>>(
                            Assign(
                                Property(node, methodInfo.Name),
                                GetServiceCall(provider, methodInfo.PropertyType)
                            ),
                            node,
                            provider
                        )
                        .Compile()
                )
        );
    }

    private static MethodCallExpression GetServiceCall(
        ParameterExpression providerParameter,
        Type serviceType
    )
    {
        return Call(
            providerParameter,
            typeof(IDependencyProvider)
                .GetMethod(nameof(IDependencyProvider.Get))!
                .MakeGenericMethod(serviceType)
        );
    }
}
