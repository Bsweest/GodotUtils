using System.Linq.Expressions;
using System.Reflection;
using GodotUtils.InstanceResolver.Models;
using static System.Linq.Expressions.Expression;

namespace GodotUtils.InstanceResolver;

/// <summary>
/// This is Singleton dependency, for register function that map dependency in node and its composite child
/// </summary>
public class InjectionContext
{
    private readonly Dictionary<Type, IStore> context = [];

    internal Store<T> GetInject<T>()
        where T : class
    {
        return (Store<T>)context[typeof(T)];
    }

    public InjectionContext Register<TDependency>()
        where TDependency : class
    {
        var method =
            typeof(TDependency)
                .GetMethods()
                .Where(method => method.GetCustomAttribute<InjectAttribute>() != null)
                .FirstOrDefault()
            ?? throw new MissingMethodException(
                $"Could not find method that has attribute [Inject] in: {nameof(TDependency)}"
            );

        var methodParamTypes = method.GetParameters().Select(param => param.ParameterType);

        var nodeLamda = Parameter(typeof(TDependency), "node");
        var providerLamda = Parameter(typeof(IDependencyProvider), "provider");

        var expression = Lambda<Action<TDependency, IDependencyProvider>>(
            Call(
                nodeLamda,
                method,
                methodParamTypes.Select(param => GetServiceCall(providerLamda, param))
            ),
            nodeLamda,
            providerLamda
        );

        context.Add(
            typeof(TDependency),
            new Store<TDependency> { InjectionMethod = expression.Compile() }
        );

        return this;
    }

    private MethodCallExpression GetServiceCall(ParameterExpression parameter, Type type)
    {
        return Call(
            parameter,
            typeof(IDependencyProvider)
                .GetMethod(nameof(IDependencyProvider.Get))!
                .MakeGenericMethod(type)
        );
    }
}
