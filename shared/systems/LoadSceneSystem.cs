using Godot;
using Shared.Buses;
using Godot.Collections;
using static Godot.ResourceLoader;

namespace Shared.Systems;

/// <summary>
/// This system is used to manage the loading of new scenes. Its mostly used by 
/// the <see cref="Entities.Screens.LoadScreenEntity"/> to control the start,
/// progress, and end of loading.
/// </summary>
public static class LoadSceneSystem
{
    private static readonly Array _progress = new();

    /// <summary>
    /// Starts loading the scene at the given path. Called in the 
    /// <see cref="Entities.Screens.LoadScreenEntity"/>.
    /// </summary>
    /// <param name="scenePath">The scene we're loading.</param>
    public static void StartLoadingScene(string scenePath)
    {
        if(HasCached(scenePath))
        { // skip loading if the scene is cached
            LoadSceneBus.BroadcastLoadingDoneEvent();
            return;
        }
        
        Error state = LoadThreadedRequest(scenePath);
        if (state == Error.Ok) 
            LoadSceneBus.BroadcastLoadingStartedEvent();
        else 
            GD.PrintErr(state);
    }
    /// <summary>
    /// Used to check the progress of the current scene that's being loaded.
    /// </summary>
    /// <param name="scenePath">The path to check the loading progress of.</param>
    /// <returns>True if we're still loading, false if loading is finished.</returns>
    public static bool LoadingInProgressCheck(string scenePath)
    {
        ThreadLoadStatus loadStatus = LoadThreadedGetStatus(scenePath, _progress);
        switch (loadStatus)
        {
            case ThreadLoadStatus.InvalidResource:
            case ThreadLoadStatus.Failed:
                GD.PrintErr(loadStatus);
                return false;
            case ThreadLoadStatus.InProgress:
                LoadSceneBus.BroadcastProgressChangedEvent(_progress[0]);
                return true;
            case ThreadLoadStatus.Loaded:
                LoadSceneBus.BroadcastProgressChangedEvent(1.0);
                LoadSceneBus.BroadcastLoadingDoneEvent();
                return false;
        }
        return true;
    }
    /// <summary>
    /// Called after the scene has been loaded. 
    /// </summary>
    /// <param name="scenePath">The scene we're </param>
    /// <returns></returns>
    public static PackedScene GetLoadedScene(string scenePath)
    {
        return (PackedScene)LoadThreadedGet(scenePath);
    }
}