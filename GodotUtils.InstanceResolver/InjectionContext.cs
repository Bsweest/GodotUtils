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
        Type genericStoreType = typeof(Store<>);

        var hasInjectAttrTypes = Assembly
            .GetCallingAssembly()
            .GetTypes()
            .Where(type =>
                type.IsClass && type.GetMembers().Any(member => member.HasInjectAttribute())
            );

        foreach (Type nodeType in hasInjectAttrTypes)
        {
            Type storeType = genericStoreType.MakeGenericType(nodeType);
            context.Add(nodeType, (IStore)Activator.CreateInstance(storeType)!);
        }
    }

    internal IStore? GetInjection(Type nodeType)
    {
        context.TryGetValue(nodeType, out var store);
        return store;
    }
}
