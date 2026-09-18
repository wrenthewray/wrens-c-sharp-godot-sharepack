using Godot;
using Shared.Resources;

namespace Shared.Components.Zoom;

/// <summary>
/// This component defines behaviour for zooming in and out based on the ZoomIn
/// and ZoomOut input actions.
/// </summary>
[GlobalClass, Icon("res://addons/at-icons/node3d/magnifying_glass.svg")]
public partial class ZoomComponent : Node
{
    /// <summary>
    /// Emitted when zoomed in or out. Use in the parent entity to control 
    /// </summary>
    /// <param name="value"></param>
    [Signal] public delegate void ValueChangedEventHandler(float value);
    private float value;
    public float Value
    {
        get => value;
        set
        {
            this.value = Mathf.Clamp(value, zoomLimit.X, zoomLimit.Y);
            EmitSignal(SignalName.ValueChanged, this.value);
        }
    }
    [Export] public bool allowZoom = true;
    [Export] private float zoomFactor = 1;
    /// <summary>
    /// The limit of zooming. Stored as degrees and converted to
    /// radians. X is the lowest angle allowed and Y is the largest
    /// angle allowed. 
    /// </summary>
    [Export (PropertyHint.Range, "1, 100")] private Vector2 zoomLimit = new(1, 10);

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(!allowZoom)
            return;

        if(Input.IsActionPressed(InputAction.ZoomIn))
            Value -= zoomFactor * (float) delta;
        else if (Input.IsActionPressed(InputAction.ZoomOut))
            Value += zoomFactor * (float) delta;
    }
}