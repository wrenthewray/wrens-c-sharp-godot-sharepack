using Godot;
using Shared.Buses;
using Shared.Systems;
using Shared.Resources;

namespace Shared.Entities.Cameras;

/// <summary>
/// Base class for all cameras to be used in the project. Controlled by
/// the <see cref="Camera3DEntityPrioritySystem"/>.
/// </summary>
[GlobalClass]
public abstract partial class Camera3DEntity : Camera3D
{
    /// <summary>
    /// Emitted when camera priority is updated. Used by the <see cref="Camera3DEntityPrioritySystem"/>
    /// to check and see if a new camera has the highest priority
    /// </summary>
    [Signal] public delegate void PriorityUpdatedEventHandler();
    /// <summary>
    /// Emitted when a camera is priority overrided.
    /// </summary>
    /// <param name="overridingCamera">the camera that is overriding the priority.</param>
    [Signal] public delegate void PriorityOverridedEventHandler(Camera3DEntity overridingCamera);
    [Signal] public delegate void ActiveCameraChangedEventHandler(bool isActive);
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
    [Export] internal TweenResource tween = new();

    public override void _EnterTree()
    {
        base._EnterTree();
        Camera3DEntityPrioritySystem.AddCamera(this);
        CameraBus.CameraChangedEvent += OnCameraChanged;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        CameraBus.CameraChangedEvent -= OnCameraChanged;
        Camera3DEntityPrioritySystem.RemoveCamera(this);
    }
    internal virtual void OnCameraChanged(Camera3DEntity toCamera3D)
    {
        EmitSignal(SignalName.ActiveCameraChanged, CameraIsActive());
    }
    internal bool CameraIsActive()
    {
        return Camera3DEntityPrioritySystem.CameraIsCurrentCamera(this);
    }
}