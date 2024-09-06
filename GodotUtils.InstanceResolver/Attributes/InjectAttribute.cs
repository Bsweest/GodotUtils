#pragma warning disable IDE0130

namespace GodotUtils.InstanceResolver;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
public class InjectAttribute : Attribute { }
