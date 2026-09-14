using Godot;

namespace Shared.Resources.Data;

/// <summary>
/// This class stores data used when tweening.
/// </summary>
[GlobalClass, Icon("res://addons/at-icons/node3d/star.svg")]
public partial class TweenResource : Resource
{
    [Export] public float duration = 1;
    [Export] public Tween.TransitionType transitionType;
    [Export] public Tween.EaseType easeType;
}