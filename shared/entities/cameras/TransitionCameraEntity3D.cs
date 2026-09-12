using Godot;
using Shared.Managers.Cameras;

namespace Shared.Entities.Cameras;

/// <summary>
/// A camera that is only used to transition between other cameras. It
/// facilititates transitions and is not meant to be a permanent camera. 
/// It is controlled by the <see cref="CameraEntity3DManager"/>. It is 
/// autoloaded.
/// </summary>
public partial class TransitionCameraEntity3D : Camera3D
{
    public static TransitionCameraEntity3D Instance;
    public override void _EnterTree()
    {
        base._EnterTree();

        CameraEntity3DManager.BeginChangeCameraEvent += TransitionToCamera;
        Instance = this;
    }
    public override void _Ready()
    {
        base._Ready();
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        CameraEntity3DManager.BeginChangeCameraEvent -= TransitionToCamera;
    }

    /// <summary>
    /// Called whenever a camera transition is requested. This camera will
    /// take the properties of the current camera and tween to the properties 
    /// of the new camera.
    /// </summary>
    /// <param name="toCamera3D">the camera to transition to.</param>
    internal async void TransitionToCamera(CameraEntity3D toCamera3D)
    { 
        if(toCamera3D.tweenTo == false)
        { // skip transition when the camera is set to not tween to. 
          // This is useful for cutscenes and other situations where 
          // you want to instantly switch cameras.
            CameraEntity3DManager.BroadcastCameraChangedEvent(toCamera3D);
            return;
        }

        Camera3D fromCamera3D;
        if(CameraEntity3DManager.CameraIsCurrentCamera(toCamera3D) 
            || CameraEntity3DManager.CurrentCamera == null)
            fromCamera3D = this;
        else 
            fromCamera3D = CameraEntity3DManager.CurrentCamera;

        float duration = toCamera3D.tween.duration;
        Tween.TransitionType transitionType = toCamera3D.tween.transitionType;
        Tween.EaseType easeType = toCamera3D.tween.easeType;

        Projection = toCamera3D.Projection;
        Environment = fromCamera3D.Environment;
        GlobalTransform = fromCamera3D.GlobalTransform;
        Fov = fromCamera3D.Fov;
        Size = fromCamera3D.Size;
        Near = fromCamera3D.Near;
        Far = fromCamera3D.Far;
        HOffset = fromCamera3D.HOffset;
        VOffset = fromCamera3D.VOffset;
        
        Transform3D targetTransform = toCamera3D.GlobalTransform;
        float targetFov = toCamera3D.Fov;
        float targetSize = toCamera3D.Size;
        float targetNear = toCamera3D.Near;
        float targetFar = toCamera3D.Far;
        float targetHOffset = toCamera3D.HOffset;
        float targetVOffset = toCamera3D.VOffset;

        MakeCurrent();
        var transitionTween = CreateTween();
        var transitionFovTween = CreateTween();
        var transitionSizeTween = CreateTween();
        var transitionNearTween = CreateTween();
        var transitionFarTween = CreateTween();
        var transitionHOffsetTween = CreateTween();
        var transitionVOffsetTween = CreateTween();

        transitionTween.TweenProperty(this, "global_transform", targetTransform, duration).SetTrans(transitionType).SetEase(easeType);
        transitionFovTween.TweenProperty(this, "fov", targetFov, duration).SetTrans(transitionType).SetEase(easeType);
        transitionSizeTween.TweenProperty(this, "size", targetSize, duration).SetTrans(transitionType).SetEase(easeType);
        transitionNearTween.TweenProperty(this, "near", targetNear, duration).SetTrans(transitionType).SetEase(easeType);
        transitionFarTween.TweenProperty(this, "far", targetFar, duration).SetTrans(transitionType).SetEase(easeType);
        transitionHOffsetTween.TweenProperty(this, "h_offset", targetHOffset, duration).SetTrans(transitionType).SetEase(easeType);
        transitionVOffsetTween.TweenProperty(this, "v_offset", targetVOffset, duration).SetTrans(transitionType).SetEase(easeType);
        
        await ToSignal(transitionTween, Tween.SignalName.Finished);
        CameraEntity3DManager.BroadcastCameraChangedEvent(toCamera3D);
    }
}