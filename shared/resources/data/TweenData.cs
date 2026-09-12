using Godot;

namespace Shared.Resources.Data;

[GlobalClass]
public partial class TweenData : Resource
{
    [Export] public float duration = 1;
    [Export] public Tween.TransitionType transitionType;
    [Export] public Tween.EaseType easeType;
}