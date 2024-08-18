using Godot;
using GodotUtils.InstanceResolver.Internal;
using static Godot.ResourceLoader;

namespace GodotUtils.InstanceResolver;

public class PackedScene<TNode, TParams>(
    string path,
    string? typehint = null,
    CacheMode cacheMode = CacheMode.Reuse
)
    where TNode : Node, IResolvableNode<TNode, TParams>
    where TParams : IParametersBuilder<TNode>
{
    private readonly PackedScene _packedScene = Load<PackedScene>(path, typehint, cacheMode);

    public PackedScene Value => _packedScene;
}
