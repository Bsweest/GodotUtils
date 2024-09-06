# Godot Utilities

## Instance Resolver

Package for Godot C#. Provides a convenience and reliable way to pass parameters for `Instantiate<Node>()` from PackScene

<details>
<summary>Main Usage</summary>

```C#
using Godot;
using GodotUtils.InstanceResolver;

namespace YourCompany.YourGame;

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
```

Above code will generate a partial class below

```C#
namespace YourGame.Components;

partial class Bullet : GodotUtils.InstanceResolver.__Internal.IResolvableNode<Bullet, Bullet.ParametersBuilder>
{
    public class ParametersBuilder : GodotUtils.InstanceResolver.__Internal.IParametersBuilder<Bullet>
    {
        public required int Damage { get; init; }

        private readonly GodotUtils.InstanceResolver.__Internal.Models.OptionalValue<int> _speedWrapper = new();
        public bool IsSpeedInitialized() => _speedWrapper.IsInitialized;
        public int Speed
        {
            get => _speedWrapper.Value;
            init
            {
                _speedWrapper.Set(value);
            }
        }

        public required global::Godot.Texture Texture { get; init; }
    }

    public Bullet Build(ParametersBuilder parameters)
    {
        _damage = parameters.Damage;
        if (parameters.IsSpeedInitialized())
        {
            _speed = parameters.Speed;
        }

        _texture = parameters.Texture;
        return this;
    }
}
```

And then you use this code to Instantiate node

```C#
readonly PackScene BULLET_INSTANCE = new PackedScene<Bullet, Bullet.ParametersBuilder>("<<uid_or_path_of_node_scene>>");

var bullet = BULLET_INSTANCE.Init<Bullet>(new() { Damage = 10, Texture = texture });
```

With Node with out a Parameter, you can use `new PackedScene<NoParamNode>().Init()`
</details>

<details>
<summary>Property Injection</summary>

Pass down service to a node and its children with any dependency injection container you want. First, implement `IDependencyProvider` as your service provider then register: `InjectionContext` class as singleton lifetime service, `Resolver` and `IDependencyProvider` should be scoped lifetime

```C#
//  Character
//  |-> Body
//  |-> Skill
//  |-> StateMachine
//  |-> AnimationPlayer
//  |-> ...

class Skill : Node
{
    [Inject]
    public StatManager _statManager;

    [Inject]
    public Resolver _resolver;

    // Or use method

    private DamangeDealer _damageDealer;

    [Inject]
    public void Inject(DamangeDealer damageDealer)
    {
        _damageDealer = damageDealer;
    }
}

resolver.Inject(node);
```

This feature is just for convenient purpose and should be used to pass down generic services that is not too specific to a Node. Other dependents should be added by [Export] or composite child nodes in Godot.


</details>
