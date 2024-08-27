namespace GodotUtils.InstanceResolver;

public interface IDependencyProvider
{
    public T Get<T>();
}
