using GodotUtils.InstanceResolver.UnitTests.Injection;

namespace GodotUtils.InstanceResolver.UnitTests
{
    public class UnitTests
    {
        public void InitPackedScene_Test() { }

        [Fact]
        public void Injection_Implement_Test()
        {
            var context = new InjectionContext().Register<Injection_Test>();
            var provider = new ServiceProvider();

            var resolver = new Resolver(context, provider);
            var node = new Injection_Test();
            resolver.Inject(node);

            Assert.NotNull(node._service1);
            Assert.NotNull(node._service2);
        }
    }
}
