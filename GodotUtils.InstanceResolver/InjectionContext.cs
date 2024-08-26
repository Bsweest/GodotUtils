using System.Reflection;
using static System.Linq.Expressions.Expression;
using GDNode = Godot.Node;

namespace GodotUtils.InstanceResolver;

/// <summary>
/// This is Singleton dependency, for register function that map dependency in node and its composite child
/// </summary>
public class InjectionContext
{
    private readonly Dictionary<Type, Action<GDNode, IDependencyProvider>> context = [];

    public InjectionContext Register<T>()
        where T : GDNode
    {
        var method =
            typeof(T)
                .GetMethods()
                .Where(method => method.GetCustomAttribute<InjectAttribute>() != null)
                .FirstOrDefault()
            ?? throw new MissingMethodException(
                $"Could not find method that has attribute [Inject] in: {nameof(T)}"
            );

        var nodeParam = Parameter(typeof(T), "node");
        var providerParam = Parameter(typeof(IDependencyProvider), "provider");

        var expression = Lambda<Action<GDNode, IDependencyProvider>>(
            Call(nodeParam, method, providerParam),
            nodeParam,
            providerParam
        );

        return this;
    }
}
