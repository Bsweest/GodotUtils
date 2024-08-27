namespace GodotUtils.InstanceResolver.Models;

internal interface IStore { }

internal class Store<T> : IStore
{
    public required Action<T, IDependencyProvider> InjectionMethod { get; init; }
}
