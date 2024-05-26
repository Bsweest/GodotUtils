using Godot;

namespace GodotUtils.InstanceResolver.UnitTests
{
    public partial class Parameter_Test : Godot.Node, IParamsResolveNode
    {
        public string name = "test";

        private int go;

        [Parameter]
        private TextureRect textureRect;

        [Parameter]
        private string Test = null!;
    }
}