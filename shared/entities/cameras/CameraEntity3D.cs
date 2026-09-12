using Godot;
using Shared.Managers.Cameras;
using Shared.Resources.Data;

namespace Shared.Entities.Cameras;

/// <summary>
/// Base class for all cameras to be used in the project. Controlled by
/// the <see cref="CameraEntity3DManager"/>.
/// </summary>
[GlobalClass]
public abstract partial class CameraEntity3D : Camera3D
{
    /// <summary>
    /// Emitted when camera priority is updated. Used by the <see cref="CameraEntity3DManager"/>
    /// to check and see if a new camera has the highest priority
    /// </summary>
    [Signal] public delegate void PriorityUpdatedEventHandler();
    /// <summary>
    /// Emitted when a camera is priority overrided.
    /// </summary>
    /// <param name="overridingCamera">the camera that is overriding the priority.</param>
    [Signal] public delegate void PriorityOverridedEventHandler(CameraEntity3D overridingCamera);
    private int priority;
    private bool overridePriority;
    /// <summary>
    /// Indicates whether this camera should override the priority of other cameras. Emits
    /// the <see cref="PriorityOverrided"/> signal when set.
    /// </summary>
    [Export] internal bool OverridePriority
    {
        get => overridePriority;
        set
        {
            
            if(value == false && overridePriority == true)
            {
                overridePriority = value;
                EmitSignal(SignalName.PriorityOverrided, this);
                return;
            }
            overridePriority = value;
            if (value == true)
                EmitSignal(SignalName.PriorityOverrided, this);
        }
    }
    /// <summary>
    /// Indicates the priority at which to display the camera. Emits
    /// the <see cref="PriorityUpdated"/> signal when set. The camera
    /// with the highest priority is the one that is displayed.
    /// </summary>
    [Export] public int Priority 
    { 
        get => priority; 
        set
        {
            priority = value;
            EmitSignal(SignalName.PriorityUpdated);
        } 
    }
    /// <summary>
    /// Indicates whether this camera should tween to the new camera when it is set as the current camera.
    /// </summary>
    [Export] internal bool tweenOnLoad = false;
    [Export] internal bool tweenTo = true;
    [Export] internal TweenData tween = new();

    public override void _Ready()
    {
        base._Ready();
    }
    public override void _EnterTree()
    {
        base._EnterTree();
        CameraEntity3DManager.AddCamera(this);
        CameraEntity3DManager.CameraChangedEvent += OnCameraChanged;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        CameraEntity3DManager.RemoveCamera(this);
        CameraEntity3DManager.CameraChangedEvent -= OnCameraChanged;
    }
    internal virtual void OnCameraChanged(CameraEntity3D toCamera3D)
    {
        // empty for now
    }
}