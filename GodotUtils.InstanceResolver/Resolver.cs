namespace GodotUtils.InstanceResolver;

/// <summary>
/// Scoped lifetime as your implement of <see cref="IDependencyProvider"></see>
/// </summary>
/// <param name="injectionContext"></param>
/// <param name="provider"></param>
public class Resolver(InjectionContext injectionContext, IDependencyProvider provider)
{
    private readonly IDependencyProvider _provider = provider;
    private readonly InjectionContext _injectionContext = injectionContext;

    public TNode Inject<TNode>(TNode node)
        where TNode : class
    {
        _injectionContext.GetInject<TNode>().InjectionMethod.Invoke(node, _provider);
        return node;
    }
}
