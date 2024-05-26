
using Godot;
using GodotUtils.InstanceResolver;
using GodotUtils.InstanceResolver.UnitTests;

var p = new PackedScene();

var x = p.Init<Parameter_Test>(i => i.Map(new() { Name = "dsad" }));

var y = p.Init<Test>();