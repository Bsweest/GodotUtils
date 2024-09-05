using System.Reflection;
using GodotUtils.InstanceResolver.Extensions;
using GodotUtils.InstanceResolver.Models;

namespace GodotUtils.InstanceResolver;

/// <summary>
/// This is Singleton dependency, for register function that map dependency in node and its composite child
/// </summary>
public class InjectionContext
{
    private readonly Dictionary<Type, IStore> context = [];

    public InjectionContext()
    {
        var store = typeof(Store<>);

        var hasInjectAttrTypes = Assembly
            .GetCallingAssembly()
            .GetTypes()
            .Where(type => type.GetMembers().Any(member => member.HasInjectAttribute()));

        foreach (var nodeType in hasInjectAttrTypes)
        {
            var storeType = store.MakeGenericType(nodeType);
            context.Add(nodeType, (IStore)Activator.CreateInstance(storeType)!);
        }
    }

    internal Store<T> GetInjection<T>()
        where T : class
    {
        return (Store<T>)context[typeof(T)];
    }
}
