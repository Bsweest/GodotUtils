using Godot;
using GodotUtils.InstanceResolver.__Internal;

namespace GodotUtils.InstanceResolver;

public static class PackedSceneExtensions
{
    public static TNode Init<TNode>(this PackedScene<TNode> packedScene)
        where TNode : Node, IResolvableNode<TNode>
    {
        return packedScene.Value.Instantiate<TNode>();
    }

    public static TNode Init<TNode, TParams>(
        this PackedScene<TNode, TParams> packedScene,
        TParams parameterBuilder
    )
        where TNode : Node, IResolvableNode<TNode, TParams>
        where TParams : IParametersBuilder<TNode>
    {
        var node = packedScene.Value.Instantiate<TNode>();
        return node.Build(parameterBuilder);
    }
}
