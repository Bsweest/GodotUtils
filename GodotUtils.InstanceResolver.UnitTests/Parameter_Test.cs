using Godot;

namespace GodotUtils.InstanceResolver.UnitTests
{
    public partial class Parameter_Test : Godot.Node, IHasResolvedParams
    {
        [Parameter]
        public string name = "test";

        private int go;

        [Parameter]
        private TextureRect textureRect = null!;

        [Parameter]
        private string _test;
    }
}