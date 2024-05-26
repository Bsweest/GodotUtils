namespace GodotUtils.InstanceResolver;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ParameterAttribute(bool isRequired = true) : Attribute
{
    public bool IsRequired { get; init; } = isRequired;
}
