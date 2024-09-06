using Godot;

namespace GodotUtils.InstanceResolver.UnitTests;

[ResolvableNode]
public partial class Bullet : RigidBody2D
{
    [Parameter]
    private int _damage;

    [Parameter]
    private int _speed = 100;

    [Parameter]
    private Texture _texture = null!;

    // game logic...
}
