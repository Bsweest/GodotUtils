namespace GodotUtils.InstanceResolver.__Internal.Models
{
    public class OptionalValue<T>
    {
        public T Value { get; private set; } = default!;
        public bool IsInitialized { get; private set; } = false;

        public void Set(T value)
        {
            IsInitialized = true;
            Value = value;
        }
    }
}
