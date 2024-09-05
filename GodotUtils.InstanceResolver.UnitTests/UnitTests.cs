using GodotUtils.InstanceResolver.UnitTests.Models;
using GodotUtils.InstanceResolver.UnitTests.Models.Injection;

namespace GodotUtils.InstanceResolver.UnitTests
{
    public class UnitTests
    {
        //public void InitPackedScene_Test() { }

        [Fact]
        public void Injection_Implement_Test()
        {
            var context = new InjectionContext();
            var provider = new ServiceProvider();

            var resolver = new Resolver(context, provider);
            var node = new UseMethodInjection();
            var node2 = new UsePropertiesInjection();

            resolver.Inject(node);
            resolver.Inject(node2);

            Assert.NotNull(node._service1);
            Assert.NotNull(node._service2);

            Assert.NotNull(node2._service1);
            Assert.NotNull(node2.Service2);
        }
    }
}
