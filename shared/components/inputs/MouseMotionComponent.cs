using Godot;

namespace Shared.Components.Inputs;

[GlobalClass, Icon("res://addons/at-icons/node/mouse.svg")]
public partial class MouseMotionComponent : Node
{
    [Signal] public delegate void MouseMovedEventHandler(float screenRelativeY, float screenRelativeX);
    [Export] private float mouseSensitivity = 100f;

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if(@event is InputEventMouseMotion inputEventMouseMotion)
        {
            EmitSignal(
                SignalName.MouseMoved, 
                inputEventMouseMotion.ScreenRelative.Y * mouseSensitivity, 
                inputEventMouseMotion.ScreenRelative.X * mouseSensitivity
            );
        }
    }
}