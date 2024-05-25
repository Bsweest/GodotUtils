using GodotUtils.InstanceResolver;

namespace UnitTests
{
#pragma warning disable CS0436 // Type conflicts with imported type
    public partial class Parameter_Test : Godot.Node, IParamsResolveNode
#pragma warning restore CS0436 // Type conflicts with imported type
    {
        [Parameter]
        public string name = "test";
    }
}