using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Shared.Entities.Cameras;

namespace Shared.Managers.Cameras;

/// <summary>
/// This manager controls all of the camera entities in the project. When
/// the entities are created and enter the tree, they add themselves to
/// the list of cameras. When they exit the tree, they remove themselves
/// from the list of cameras. 
/// </summary>
public static class CameraEntity3DManager
{
    private static bool priorityOverrided = false;
    private static readonly List<CameraEntity3D> cameras = new();

    private static CameraEntity3D currentCamera;
    private static Action<CameraEntity3D> beginChangeCameraEvent;
    private static Action<CameraEntity3D> cameraChangedEvent = SetCurrentCamera;

    /// <summary>
    /// The current camera that the player is seeing from. When set,
    /// <see cref="Camera3D.MakeCurrent()"/> is called.
    /// </summary>
    public static CameraEntity3D CurrentCamera 
    {
        get => currentCamera;
        private set
        {
            currentCamera = value;
            currentCamera.MakeCurrent();
        }
    }
    /// <summary>
    /// Broadcast when changing to a new camera. Attached in the 
    /// <see cref="TransitionCameraEntity3D"/> 
    /// </summary>
    public static Action<CameraEntity3D> BeginChangeCameraEvent { get => beginChangeCameraEvent; set => beginChangeCameraEvent = value; }
    /// <summary>
    /// Broadcast when we're finished changing cameras. Attached to
    /// the <see cref="SetCurrentCamera"/>
    /// </summary>
    public static Action<CameraEntity3D> CameraChangedEvent { get => cameraChangedEvent; set => cameraChangedEvent = value; }

    /// <summary>
    /// Called by a <see cref="CameraEntity3D"/> when it enters the tree. Adds the camera to the list of cameras.
    /// </summary>
    /// <param name="camera"></param>
    /// <exception cref="Exception"></exception>
    public static void AddCamera(CameraEntity3D camera)
    {
        if(CameraIsInList(camera))
            throw new Exception($"Camera {camera.Name} attempted adding, but it is already in the list of cameras!");
        cameras.Add(camera);
        OnCameraAdded(camera);
    }
    /// <summary>
    /// Called by a <see cref="CameraEntity3D"/> when it exits the tree. Removes the camera from the list of cameras.
    /// </summary>
    /// <param name="camera"></param>
    /// <exception cref="Exception"></exception>
    public static void RemoveCamera(CameraEntity3D camera)
    {
        if(!CameraIsInList(camera))
            throw new Exception($"Camera {camera.Name} attempted removal, but it isn't in the list of cameras!");
        cameras.Remove(camera);
        OnCameraRemoved(camera);
    }
    
    public static void BroadcastBeginChangeCameraEvent(CameraEntity3D toCamera3D)
    {
        BeginChangeCameraEvent?.Invoke(toCamera3D);
    }

    public static void BroadcastCameraChangedEvent(CameraEntity3D toCamera3D)
    {
        CameraChangedEvent?.Invoke(toCamera3D);
    }
    /// <summary>
    /// Internal method that sets the current camera. Triggered 
    /// by the <see cref="CameraChangedEvent"/>.
    /// </summary>
    /// <param name="toCamera3D">The camera to change to.</param>
    private static void SetCurrentCamera(CameraEntity3D toCamera3D)
    {
        CurrentCamera = toCamera3D;
    }
    /// <summary>
    /// Returns true if the given camera is the current camera. False otherwise.
    /// </summary>
    /// <param name="camera">The camera to check.</param>
    public static bool CameraIsCurrentCamera(CameraEntity3D camera)
    {
        return CurrentCamera == camera;
    }

    private static bool CameraIsInList(CameraEntity3D camera)
    {
        return cameras.Contains(camera);
    }

    private static void OnPriorityUpdated()
    {
        if(priorityOverrided)
            return;
        CameraEntity3D highestPriorityCamera = CurrentCamera;
        foreach(CameraEntity3D camera in cameras)
        {
            if(camera == highestPriorityCamera)
                continue;
            if(camera.Priority > highestPriorityCamera.Priority)
                highestPriorityCamera = camera;
        }
        if(highestPriorityCamera != CurrentCamera)
            BroadcastBeginChangeCameraEvent(highestPriorityCamera);
    }
    private static void OnPriorityOverride(CameraEntity3D prioritizedCamera)
    {
        foreach(CameraEntity3D camera in cameras)
        {
            if(camera == prioritizedCamera)
                continue;
            camera.OverridePriority = false;
        }

        if(prioritizedCamera.OverridePriority == false)
        {
            priorityOverrided = false;
            OnPriorityUpdated();
            return;
        }

        priorityOverrided = true;
        if(!CameraIsCurrentCamera(prioritizedCamera))
            BroadcastBeginChangeCameraEvent(prioritizedCamera);
    }
    private async static void OnCameraAdded(CameraEntity3D cameraEntity3D)
    {
        cameraEntity3D.PriorityOverrided += OnPriorityOverride;
        cameraEntity3D.PriorityUpdated += OnPriorityUpdated;

        CameraEntity3D highestPriorityCamera = cameras.MaxBy(camera => camera.Priority);
        if(await DoubleCheckPriorityOnCameraAdded(highestPriorityCamera))
            if(highestPriorityCamera.tweenOnLoad)
                BroadcastBeginChangeCameraEvent(highestPriorityCamera);
            else
                BroadcastCameraChangedEvent(highestPriorityCamera);
        
    }
    private async static Task<bool> DoubleCheckPriorityOnCameraAdded(CameraEntity3D cameraEntity3D)
    {
        await Task.Delay(250);
        CameraEntity3D highestPriorityCamera = cameras.MaxBy(camera => camera.Priority);
        return highestPriorityCamera == cameraEntity3D;
    }
    private static void OnCameraRemoved(CameraEntity3D cameraEntity3D)
    {
        cameraEntity3D.PriorityOverrided -= OnPriorityOverride;
        cameraEntity3D.PriorityUpdated -= OnPriorityUpdated;

        OnPriorityUpdated();
    }
}