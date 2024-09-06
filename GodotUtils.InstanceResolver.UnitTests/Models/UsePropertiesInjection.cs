using Godot;
using GodotUtils.InstanceResolver.UnitTests.Models.Injection;

namespace GodotUtils.InstanceResolver.UnitTests.Models
{
    internal partial class UsePropertiesInjection : Node
    {
        [Inject]
        public FirstService _service1;

        [Inject]
        public SecondService Service2 { get; set; }
    }
}
