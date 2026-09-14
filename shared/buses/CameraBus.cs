using System;
using Shared.Entities.Cameras;
using Shared.Managers.Cameras;

namespace Shared.Buses;

/// <summary>
/// This class stores events used by camera entities and the camera manager to 
/// communicate with each other.
/// </summary>
public static class CameraBus
{
    private static Action<CameraEntity3D> beginChangeCameraEvent;
    private static Action<CameraEntity3D> cameraChangedEvent = CameraEntity3DManager.SetCurrentCamera;

    /// <summary>
    /// Broadcast when we begin changing cameras, so we can transition between the
    /// two cameras. Attached to the <see cref="TransitionCameraEntity3D.TransitionToCamera"/>
    /// method.
    /// </summary>
    public static Action<CameraEntity3D> BeginChangeCameraEvent { get => beginChangeCameraEvent; set => beginChangeCameraEvent = value; }
    /// <summary>
    /// Broadcast when the <see cref="TransitionCameraEntity3D.TransitionToCamera"/> 
    /// finishes running, signaling the camera manager to switch cameras. Attached to
    /// the <see cref="CameraEntity3DManager.SetCurrentCamera"/> method.
    /// </summary>
    public static Action<CameraEntity3D> CameraChangedEvent { get => cameraChangedEvent; set => cameraChangedEvent = value; }

    public static void BroadcastBeginChangeCameraEvent(CameraEntity3D toCamera3D)
    {
        BeginChangeCameraEvent?.Invoke(toCamera3D);
    }

    public static void BroadcastCameraChangedEvent(CameraEntity3D toCamera3D)
    {
        CameraChangedEvent?.Invoke(toCamera3D);
    }
}