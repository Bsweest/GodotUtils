using Godot;

namespace GodotUtils.InstanceResolver.Models;

internal interface IStore
{
    void Inject(Node node, IDependencyProvider provider);
}
