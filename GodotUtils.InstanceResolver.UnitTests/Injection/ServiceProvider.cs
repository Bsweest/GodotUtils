namespace GodotUtils.InstanceResolver.UnitTests.Injection
{
    internal class ServiceProvider : IDependencyProvider
    {
        private readonly Dictionary<Type, dynamic> container =
            new()
            {
                { typeof(FirstService), new FirstService() },
                { typeof(SecondService), new SecondService() }
            };

        public T Get<T>()
        {
            return container[typeof(T)];
        }
    }
}
