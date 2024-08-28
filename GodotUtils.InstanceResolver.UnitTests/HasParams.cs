﻿using Godot;

namespace GodotUtils.InstanceResolver.UnitTests;

[ResolvableNode]
internal partial class HasParams : Node
{
    [Parameter]
    private string value = "not required";

    [Parameter]
    private List<int> list = null!;
}
