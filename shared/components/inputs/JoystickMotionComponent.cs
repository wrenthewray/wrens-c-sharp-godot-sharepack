using Godot;
using Shared.Resources;

namespace Shared.Components.Inputs;

[GlobalClass, Icon("res://addons/at-icons/node/joystick.svg")]
public partial class JoystickMotionComponent : Node
{
    [Signal] public delegate void JoystickMovedEventHandler(float screenRelativeY, float screenRelativeX);
    [Export] private float joystickSensitivity = 1f;

    [Export] public JoystickType joystickType = JoystickType.LEFT;

    private Vector2 motionVector = new();
    public override void _Process(double delta)
    {
        base._Process(delta);

        Vector2 inputVector;
        if(joystickType == JoystickType.LEFT)
            inputVector = Input.GetVector(
                InputAction.LJSLeft,
                InputAction.LJSRight,
                InputAction.LJSUp,
                InputAction.LJSDown
            );
        else
            inputVector = Input.GetVector(
                InputAction.RJSLeft,
                InputAction.RJSRight,
                InputAction.RJSUp,
                InputAction.RJSDown
            );
        
        if(inputVector != motionVector)
            ChangeMotionVector(inputVector);
    }

    public void ChangeMotionVector(Vector2 newVector)
    {
        motionVector = newVector;
        EmitSignal(SignalName.JoystickMoved, motionVector.Y, motionVector.X);
    }
}

public enum JoystickType
{
    RIGHT,
    LEFT
}