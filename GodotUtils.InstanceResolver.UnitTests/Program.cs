using Godot;
using GodotUtils.InstanceResolver;
using GodotUtils.InstanceResolver.UnitTests;

var p = new PackedScene();

var x = p.Init<Parameter_Test>(item => item.Map(new() { Test = "Hello World" }));
var y = p.Init<Test>();