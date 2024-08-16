using Godot;
using GodotUtils.InstanceResolver.Internal;

namespace GodotUtils.InstanceResolver;

public class PackedScene<T>(PackedScene resource)
    where T : IInstance
{
    public PackedScene Value => resource;
}
