namespace GodotUtils.InstanceResolver.UnitTests
{
    public class UnitTests
    {
        public void InitPackedScene_Test()
        {
            new PackedScene<Generated, Generated.ParametersBuilder>("").Init(
                new() { Pos = Godot.Vector2.Zero, Value = "name" }
            );
        }
    }
}
