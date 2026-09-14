using Godot;
using Shared.Buses;
using Shared.Managers.Data;


namespace Shared.Entities.Screens;

/// <summary>
/// A black screen that fades in and out, emitting a signal 
/// after fading to black and returning to normal.
/// </summary>
public partial class LoadScreenEntity : CanvasLayer
{
    [Export] private AnimationPlayer animator;
    [Export] private ProgressBar progressBar;
    private bool loading = false;

    private string scenePath;

    public override void _Ready()
    {
        LoadScreenBus.LoadNewSceneEvent += LoadNewScene;
        LoadScreenBus.LoadingStartedEvent += () => loading = true;

        LoadScreenBus.ProgressChangedEvent += ProgressChanged;
        LoadScreenBus.LoadingDoneEvent += LoadingDone;
        animator.AnimationFinished += AnimationFinished;

        animator.Queue(ScreenAnimation.Reset);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        CheckProgress();
    }
    private void AnimationFinished(StringName animName)
    {
        if (animName == ScreenAnimation.FadeToBlack) 
            LoadScreenManager.StartLoadingScene(scenePath);
        else if (animName == ScreenAnimation.FadeAway) 
            animator.Queue(ScreenAnimation.Reset);
    }
    private void CheckProgress()
    {
        if (!loading) 
            return;
        loading = LoadScreenManager.LoadProgress(scenePath);
    }

    public void LoadNewScene(string scenePath)
    {
        progressBar.Value = 0;
        this.scenePath = scenePath;
        animator.Queue(ScreenAnimation.FadeToBlack);
    }
    private void ProgressChanged(Variant progress)
    {
        if (loading == false) 
            loading = true;
        if (Mathf.Abs(progressBar.Value - (float)progress * 100f) >= 0.5f)
            progressBar.Value = (float)progress * 100;
        
    }
    public void LoadingDone()
    {
        GetTree().ChangeSceneToPacked(LoadScreenManager.GetLoadedScene(scenePath));
        animator.Queue(ScreenAnimation.FadeAway);
    }
}
public static class ScreenAnimation
{
    public static readonly string FadeToBlack = "fade_to_black";
    public static readonly string FadeAway = "fade_away";
    public static readonly string Reset = "RESET";
}