namespace GodotUtils.InstanceResolver;

public class Resolver(IDependencyProvider provider)
{
    private readonly IDependencyProvider _provider = provider;
}
