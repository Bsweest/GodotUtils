# Godot Utilities

## Instance Resolver

Package for Godot C#. Provides a convenience and reliable way to pass parameters for `Instantiate<Node>()` from PackScene

### Main Usage

```C#
using Godot;
using GodotUtils.InstanceResolver

namespace YourGame.Components;

public partial class Bullet : RigidBody2D, IParamsResolvedNode
{
    [Parameter]
    private int _damage;

    [Parameter]
    private int _speed = 100;

    [Parameter]
    private Texture _texture;

    // game logic...
}
```

Above code will generate a partial class below

```C#
namespace YourGame.Components;

partial class Bullet : GodotUtils.InstanceResolver.IResolvedNode<Bullet, Bullet.BuildParameters>
{
    public class BuildParameters
    {
        public required int Damage { get; init; }

        public readonly GodotUtils.InstanceResolver.Internal.Models.OptionalValue<int> _speedWrapper = new();
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

    public BuildParameters Map(BuildParameters parameters)
    {
        _damage = parameters.Damage;

        if (parameters._speedWrapper.IsInitialized)
        {
            _speed = parameters.Speed;
        }

        _texture = parameters.Texture;
    }
}
```

And then you use this code to Instantiate node

```C#
readonly PackScene BULLET_INSTANCE = ResourceLoader.Load("<<uid_or_path_of_node_scene>>");

var bullet = BULLET_INSTANCE.Init<Bullet>(node => node.Map(new() { Damage = 10, Texture = texture }));
```

With Node Scene with out a Parameter, you can implement `INoResolvedParams` and use directly `INSTANCE.Init<NoParamNode>()`
