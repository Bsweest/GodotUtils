using Godot;

namespace GodotUtils.InstanceResolver.UnitTests
{
    public partial class Parameter_Test : Godot.Node, IParamsResolveNode
    {
        [Parameter]
        public string name = "test";

        [Parameter]
        private int go;

        [Parameter]
        private TextureRect textureRect;
    }
}