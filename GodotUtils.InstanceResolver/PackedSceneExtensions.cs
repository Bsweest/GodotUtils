using Godot;
using GodotUtils.InstanceResolver.Internal;

namespace GodotUtils.InstanceResolver;

public static class PackedSceneExtensions
{
    public static TNode Init<TNode>(this PackedScene<TNode> packedScene)
        where TNode : Node, INoParamsInstance
    {
        return packedScene.Value.Instantiate<TNode>();
    }

    public static TNode Init<TNode, TParamsBuilder>(
        this PackedScene<TNode> packedScene,
        TParamsBuilder builder
    )
        where TNode : Node, IHasParamsInstance
        where TParamsBuilder : IParametersBuilder<TNode>
    {
        var node = packedScene.Value.Instantiate<TNode>();

        return node;
    }
}
