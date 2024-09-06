using System.Reflection;

namespace GodotUtils.InstanceResolver.Extensions;

internal static class MemberInfoExtensions
{
    public static bool HasInjectAttribute(this MemberInfo member)
    {
        return member.GetCustomAttribute<InjectAttribute>() != null;
    }
}
