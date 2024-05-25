namespace GodotUtils.InstanceResolver.Generator.Constants;

internal static class ClassNameConst
{
    private static readonly string resolvedNodeNamespace = $"GodotUtils.InstanceResolver.Internal";
    private static readonly string resolvedNodeName = "IResolveNode";

    public const string BuildParametersClassName = "BuildParameters";
    public const string PassingObj = "parameters";

    public static string RequiredInterface(string className)
    {
        return $"{resolvedNodeNamespace}.{resolvedNodeName}<{className}>";
    }
}
