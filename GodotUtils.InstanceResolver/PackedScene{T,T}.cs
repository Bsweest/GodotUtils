using Godot;
using GodotUtils.InstanceResolver.__Internal;

namespace GodotUtils.InstanceResolver;

public class PackedScene<TNode, TParams>(
    string path,
    string? typehint = null,
    ResourceLoader.CacheMode cacheMode = ResourceLoader.CacheMode.Reuse
) : ResolvablePackedScene(path, typehint, cacheMode)
    where TNode : Node, IResolvableNode<TNode, TParams>
    where TParams : IParametersBuilder<TNode> { }
