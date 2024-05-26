namespace GodotUtils.InstanceResolver;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class ParameterAttribute : Attribute
{
    public bool IsRequired { get; private set; } = true;

    public ParameterAttribute(bool isRequired)
    {
        IsRequired = isRequired;
    }

    public ParameterAttribute() { }
}
