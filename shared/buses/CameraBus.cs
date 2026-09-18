using System;
using Shared.Entities.Cameras;
using Shared.Systems;

namespace Shared.Buses;

/// <summary>
/// This class stores events used by camera entities and the camera manager to 
/// communicate with each other.
/// </summary>
public static class CameraBus
{
    private static Action<Camera3DEntity> beginChangeCameraEvent;
    private static Action<Camera3DEntity> cameraChangedEvent = Camera3DEntityPrioritySystem.SetCurrentCamera;

    /// <summary>
    /// Broadcast when we begin changing cameras, so we can transition between the
    /// two cameras. Attached to the <see cref="TransitionCamera3DEntity.TransitionToCamera"/>
    /// method.
    /// </summary>
    public static Action<Camera3DEntity> BeginChangeCameraEvent { get => beginChangeCameraEvent; set => beginChangeCameraEvent = value; }
    /// <summary>
    /// Broadcast when the <see cref="TransitionCamera3DEntity.TransitionToCamera"/> 
    /// finishes running, signaling the camera manager to switch cameras. Attached to
    /// the <see cref="Camera3DEntityPrioritySystem.SetCurrentCamera"/> method.
    /// </summary>
    public static Action<Camera3DEntity> CameraChangedEvent { get => cameraChangedEvent; set => cameraChangedEvent = value; }

    public static void BroadcastBeginChangeCameraEvent(Camera3DEntity toCamera3D)
    {
        BeginChangeCameraEvent?.Invoke(toCamera3D);
    }

    public static void BroadcastCameraChangedEvent(Camera3DEntity toCamera3D)
    {
        CameraChangedEvent?.Invoke(toCamera3D);
    }
}