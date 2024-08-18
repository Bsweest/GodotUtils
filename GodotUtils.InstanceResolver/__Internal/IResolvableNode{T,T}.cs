using Godot;

namespace GodotUtils.InstanceResolver.__Internal;

public interface IResolvableNode<TNode, TParams>
    where TNode : Node
    where TParams : IParametersBuilder<TNode>
{
    TNode Build(TParams parameters);
}
