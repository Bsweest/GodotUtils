using Godot;
using static Godot.ResourceLoader;

namespace GodotUtils.InstanceResolver.__Internal;

public abstract class ResolvablePackedScene(
    string path,
    string? typehint = null,
    CacheMode cacheMode = CacheMode.Reuse
)
{
    private readonly PackedScene _packedScene = Load<PackedScene>(path, typehint, cacheMode);

    public PackedScene Value => _packedScene;
}
