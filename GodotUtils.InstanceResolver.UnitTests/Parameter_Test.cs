using Godot;

namespace GodotUtils.InstanceResolver.UnitTests
{
    public partial class Parameter_Test : Godot.Node, IParamsResolveNode
    {
        [Parameter]
        public string name = "test";

        private int go;

        [Parameter]
        private TextureRect textureRect = null!;

        [Parameter]
        private string Test;
    }
}