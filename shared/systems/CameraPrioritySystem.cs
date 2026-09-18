using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Shared.Buses;
using Shared.Entities.Cameras;

namespace Shared.Systems;

/// <summary>
/// This system controls all of the camera entities in the project. When
/// the entities are created and enter the tree, they add themselves to
/// the list of cameras. When they exit the tree, they remove themselves
/// from the list of cameras. 
/// </summary>
public static class Camera3DEntityPrioritySystem
{
    private static bool priorityOverrided = false;
    private static readonly List<Camera3DEntity> cameras = new();

    private static Camera3DEntity currentCamera;

    /// <summary>
    /// The current camera that the player is seeing from. When set,
    /// <see cref="Camera3D.MakeCurrent()"/> is called.
    /// </summary>
    public static Camera3DEntity CurrentCamera 
    {
        get => currentCamera;
        private set
        {
            currentCamera = value;
            currentCamera.MakeCurrent();
        }
    }


    /// <summary>
    /// Called by a <see cref="Camera3DEntity"/> when it enters the tree. Adds the camera to the list of cameras.
    /// </summary>
    /// <param name="camera"></param>
    /// <exception cref="Exception"></exception>
    public static void AddCamera(Camera3DEntity camera)
    {
        if(CameraIsInList(camera))
            throw new Exception($"Camera {camera.Name} attempted adding, but it is already in the list of cameras!");
        cameras.Add(camera);
        OnCameraAdded(camera);
    }
    /// <summary>
    /// Called by a <see cref="Camera3DEntity"/> when it exits the tree. Removes the camera from the list of cameras.
    /// </summary>
    /// <param name="camera"></param>
    /// <exception cref="Exception"></exception>
    public static void RemoveCamera(Camera3DEntity camera)
    {
        if(!CameraIsInList(camera))
            throw new Exception($"Camera {camera.Name} attempted removal, but it isn't in the list of cameras!");
        cameras.Remove(camera);
        OnCameraRemoved(camera);
    }
    
    /// <summary>
    /// Internal method that sets the current camera. Triggered 
    /// by the <see cref="CameraBus.CameraChangedEvent"/>.
    /// </summary>
    /// <param name="toCamera3D">The camera to change to.</param>
    public static void SetCurrentCamera(Camera3DEntity toCamera3D)
    {
        CurrentCamera = toCamera3D;
    }
    /// <summary>
    /// Returns true if the given camera is the current camera. False otherwise.
    /// </summary>
    /// <param name="camera">The camera to check.</param>
    public static bool CameraIsCurrentCamera(Camera3DEntity camera)
    {
        return CurrentCamera == camera;
    }

    private static bool CameraIsInList(Camera3DEntity camera)
    {
        return cameras.Contains(camera);
    }

    private static void OnPriorityUpdated()
    {
        if(priorityOverrided)
            return;
    
        Camera3DEntity highestPriorityCamera = GetHighestPriorityCamera();
        if(highestPriorityCamera != CurrentCamera)
            CameraBus.BroadcastBeginChangeCameraEvent(highestPriorityCamera);
    }
    private static void OnPriorityOverride(Camera3DEntity prioritizedCamera)
    {
        foreach(Camera3DEntity camera in cameras)
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
            CameraBus.BroadcastBeginChangeCameraEvent(prioritizedCamera);
    }
    private async static void OnCameraAdded(Camera3DEntity cameraEntity3D)
    {
        cameraEntity3D.PriorityOverrided += OnPriorityOverride;
        cameraEntity3D.PriorityUpdated += OnPriorityUpdated;

        Camera3DEntity highestPriorityCamera = GetHighestPriorityCamera();
        if (await DoubleCheckPriorityOnCameraAdded(highestPriorityCamera))
            if (highestPriorityCamera.tweenOnLoad)
                CameraBus.BroadcastBeginChangeCameraEvent(highestPriorityCamera);
            else
                CameraBus.BroadcastCameraChangedEvent(highestPriorityCamera);

    }
    private async static Task<bool> DoubleCheckPriorityOnCameraAdded(Camera3DEntity cameraEntity3D)
    {
        await Task.Delay(250);
        Camera3DEntity highestPriorityCamera = GetHighestPriorityCamera();
        return highestPriorityCamera == cameraEntity3D;
    }

    private static Camera3DEntity GetHighestPriorityCamera()
    {
        return cameras.MaxBy(camera => camera.Priority);
    }

    private static void OnCameraRemoved(Camera3DEntity cameraEntity3D)
    {
        cameraEntity3D.PriorityOverrided -= OnPriorityOverride;
        cameraEntity3D.PriorityUpdated -= OnPriorityUpdated;

        OnPriorityUpdated();
    }
}