using Godot;

namespace GodotUtils.InstanceResolver;

/// <summary>
/// Scoped lifetime with your implement of <see cref="IDependencyProvider"></see>
/// </summary>
/// <param name="injectionContext"></param>
/// <param name="provider"></param>
public class Resolver(InjectionContext injectionContext, IDependencyProvider provider)
{
    private readonly IDependencyProvider _provider = provider;
    private readonly InjectionContext _injectionContext = injectionContext;

    public TNode Inject<TNode>(TNode node)
        where TNode : Node
    {
        _injectionContext.GetInjection(node.GetType())?.Inject(node, _provider);

        foreach (var child in node.GetChildren())
        {
            Inject(child);
        }

        return node;
    }
}
