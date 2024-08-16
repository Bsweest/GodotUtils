using Godot;

namespace GodotUtils.InstanceResolver.Internal;

public interface IResolvedNode<TNode, TParams>
    where TParams : IParametersBuilder<TNode>
    where TNode : Node
{
    TParams Map(TParams parameters);
}
