using Godot;
using GodotUtils.InstanceResolver.Internal;

namespace GodotUtils.InstanceResolver;

public static class Resolver
{
    public static TNode Init<TNode>(this PackedScene packedScene)
        where TNode : Node, INoResolvedParams
    {
        return packedScene.Instantiate<TNode>();
    }

    public static TNode Init<TNode>(
        this PackedScene packedScene,
        Func<TNode, IParameters<TNode>> requiredMappingFunction
    )
        where TNode : Node, IHasResolvedParams
    {
        var node = packedScene.Instantiate<TNode>();
        requiredMappingFunction.Invoke(node);
        return node;
    }
}
