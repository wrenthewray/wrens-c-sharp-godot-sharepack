using Godot;
using Shared.Buses;
using Shared.Systems;


namespace Shared.Entities.Screens;

/// <summary>
/// A black screen that fades in and out, emitting a signal after fading to black 
/// and returning to normal.
/// </summary>
public partial class LoadScreenEntity : CanvasLayer
{
    [Export] private AnimationPlayer animator;
    [Export] private ProgressBar progressBar;
    
    private bool loading = false;
    private string scenePath;

    public override void _Ready()
    {
        animator.Queue(ScreenAnimation.Reset);
        animator.AnimationFinished += AnimationFinished;

        LoadSceneBus.LoadNewSceneEvent += LoadNewScene;
        LoadSceneBus.LoadingStartedEvent += LoadingStarted;
        LoadSceneBus.ProgressChangedEvent += ProgressChanged;
        LoadSceneBus.LoadingDoneEvent += LoadingDone;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        CheckLoadingProgress();
    }
    private void AnimationFinished(StringName animName)
    {
        if (animName == ScreenAnimation.FadeToBlack) 
            LoadSceneSystem.StartLoadingScene(scenePath);
        else if (animName == ScreenAnimation.FadeAway) 
            animator.Queue(ScreenAnimation.Reset);
    }
    private void CheckLoadingProgress()
    {
        if (!loading) 
            return;
        loading = LoadSceneSystem.LoadingInProgressCheck(scenePath);
    }

    public void LoadNewScene(string scenePath)
    {
        progressBar.Value = 0;
        this.scenePath = scenePath;
        animator.Queue(ScreenAnimation.FadeToBlack);
    }
    public void LoadingStarted()
    {
        loading = true;
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
        GetTree().ChangeSceneToPacked(LoadSceneSystem.GetLoadedScene(scenePath));
        animator.Queue(ScreenAnimation.FadeAway);
    }
}
public static class ScreenAnimation
{
    public static readonly string FadeToBlack = "fade_to_black";
    public static readonly string FadeAway = "fade_away";
    public static readonly string Reset = "RESET";
}