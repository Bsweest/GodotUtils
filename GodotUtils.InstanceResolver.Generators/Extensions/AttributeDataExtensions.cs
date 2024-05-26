using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace GodotUtils.InstanceResolver.Generators.Extensions
{
    public static class AttributeDataExtensions
    {
        public static T? GetNamedArgument<T>(this AttributeData attributeData, string name, T? fallback = default)
        {
            if (attributeData.TryGetNamedArgument(name, out T? value))
            {
                return value;
            }

            return fallback;
        }

        public static bool TryGetNamedArgument<T>(this AttributeData attributeData, string name, out T? value)
        {
            foreach (KeyValuePair<string, TypedConstant> properties in attributeData.NamedArguments)
            {
                if (properties.Key == name)
                {
                    value = (T?)properties.Value.Value;

                    return true;
                }
            }

            value = default;

            return false;
        }

        public static IEnumerable<T?> GetConstructorArguments<T>(this AttributeData attributeData)
        {
            static IEnumerable<T?> Enumerate(IEnumerable<TypedConstant> constants)
            {
                foreach (TypedConstant constant in constants)
                {
                    if (constant.IsNull)
                    {
                        yield return default;
                    }

                    if (constant.Kind == TypedConstantKind.Primitive &&
                        constant.Value is T value)
                    {
                        yield return value;
                    }
                    else if (constant.Kind == TypedConstantKind.Array)
                    {
                        foreach (T? item in Enumerate(constant.Values))
                        {
                            yield return item;
                        }
                    }
                }
            }

            return Enumerate(attributeData.ConstructorArguments);
        }
    }
}