using GodotUtils.InstanceResolver.Generator.Helper;
using System;

namespace GodotUtils.InstanceResolver.Generator.Components;

/// <summary>
/// A model representing a value and an associated set of diagnostic errors.
/// </summary>
/// <typeparam name="TValue">The type of the wrapped value.</typeparam>
/// <param name="Value">The wrapped value for the current result.</param>
/// <param name="Errors">The associated diagnostic errors, if any.</param>
internal sealed record Result<TValue>(TValue Value, EquatableArray<DiagnosticInfo> Errors)
    where TValue : IEquatable<TValue>?;
