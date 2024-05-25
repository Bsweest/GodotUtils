using Godot;

namespace GodotUtils.InstanceResolver.Internal;

public interface INoParamsResolveNode<TNode> : IResolveNode<TNode>
    where TNode : Node
{ }
