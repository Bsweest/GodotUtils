using GodotUtils.InstanceResolver.Generator.Helper;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace GodotUtils.InstanceResolver.Generator.Extensions;

internal static class ProviderExtensions
{
    public static IncrementalValuesProvider<(TKey Key, EquatableArray<TElement> Right)> GroupBy<
        TLeft,
        TRight,
        TKey,
        TElement
    >(
        this IncrementalValuesProvider<(TLeft Left, TRight Right)> source,
        Func<(TLeft Left, TRight Right), TKey> keySelector,
        Func<(TLeft Left, TRight Right), TElement> elementSelector
    )
        where TLeft : IEquatable<TLeft>
        where TRight : IEquatable<TRight>
        where TKey : IEquatable<TKey>
        where TElement : IEquatable<TElement>
    {
        return source
            .Collect()
            .SelectMany(
                (item, token) =>
                {
                    Dictionary<TKey, ImmutableArray<TElement>.Builder> map = [];

                    foreach ((TLeft, TRight) pair in item)
                    {
                        TKey key = keySelector(pair);
                        TElement element = elementSelector(pair);

                        if (!map.TryGetValue(key, out ImmutableArray<TElement>.Builder? builder))
                        {
                            builder = ImmutableArray.CreateBuilder<TElement>();

                            map.Add(key, builder);
                        }

                        builder.Add(element);
                    }

                    token.ThrowIfCancellationRequested();

                    ImmutableArray<(TKey Key, EquatableArray<TElement> Elements)>.Builder result =
                        ImmutableArray.CreateBuilder<(TKey, EquatableArray<TElement>)>();

                    foreach (KeyValuePair<TKey, ImmutableArray<TElement>.Builder> entry in map)
                    {
                        result.Add((entry.Key, entry.Value.ToImmutable()));
                    }

                    return result;
                }
            );
    }
}
