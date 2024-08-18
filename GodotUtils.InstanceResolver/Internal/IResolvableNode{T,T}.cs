using Godot;

namespace GodotUtils.InstanceResolver.Internal;

public interface IResolvableNode<TNode, TParams>
    where TNode : Node
    where TParams : IParametersBuilder<TNode>
{
    TNode Build(TParams parameters);
}
