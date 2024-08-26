using Godot;

namespace GodotUtils.InstanceResolver.UnitTests;

[ResolvableNode]
public partial class Generated : Godot.Node
{
    [Parameter]
    private string value = "not required";

    [Parameter]
    private Vector2 pos;
}
