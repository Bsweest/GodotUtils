using GodotUtils.InstanceResolver.UnitTests.Injection;

namespace GodotUtils.InstanceResolver.UnitTests
{
    internal class Injection_Test
    {
        public FirstService _service1;
        public SecondService _service2;

        [Inject]
        public void Inject(FirstService service1, SecondService service2)
        {
            _service1 = service1;
            _service2 = service2;
        }
    }
}
