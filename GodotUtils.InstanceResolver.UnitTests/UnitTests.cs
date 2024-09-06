using GodotUtils.InstanceResolver.UnitTests.Models.Injection;

namespace GodotUtils.InstanceResolver.UnitTests
{
    public class UnitTests
    {
        [Fact]
        public void Injection_Implement_Test()
        {
            var context = new InjectionContext();
            var provider = new ServiceProvider();
            var resolver = new Resolver(context, provider);
        }
    }
}
