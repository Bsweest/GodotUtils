using Godot;
using TestNamespace;

namespace GodotUtils.InstanceResolver.UnitTests
{
    public partial class Parameter_Test : Godot.Node, IHasResolvedParams
    {
        [Parameter]
        private float damage;

        [Parameter]
        private string _test = Test.Inside.Z;

        [Parameter]
        private Vector2 texture;
    }
}