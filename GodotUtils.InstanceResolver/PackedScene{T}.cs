using Godot;
using GodotUtils.InstanceResolver.__Internal;

namespace GodotUtils.InstanceResolver
{
    public class PackedScene<TNode> : ResolvablePackedScene
        where TNode : Node, IResolvableNode<TNode>
    {
        public PackedScene(
            string path,
            string? typehint = null,
            ResourceLoader.CacheMode cacheMode = ResourceLoader.CacheMode.Reuse
        )
            : base(path, typehint, cacheMode) { }
    }
}
