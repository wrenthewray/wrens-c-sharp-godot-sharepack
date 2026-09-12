using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using static Godot.ResourceLoader;
using Array = Godot.Collections.Array;

namespace Shared.Managers.Data;

/// <summary>
/// This class is used to manage the loading of new scenes. Its mostly used by 
/// the <see cref="Entities.Screens.LoadScreenEntity"/> to control the start,
/// progress, and end of loading.
/// </summary>
public static class LoadScreenManager
{
    private static readonly Array _progress = new();

    private static Action loadingStartedEvent;
    private static Action<Variant> progressChangedEvent;
    private static Action<string> loadNewSceneEvent;
    private static Action loadingDoneEvent;
    private static Action startAutosaveEvent;

    public static Action LoadingStartedEvent { get => loadingStartedEvent; set => loadingStartedEvent = value; }
    public static Action<Variant> ProgressChangedEvent { get => progressChangedEvent; set => progressChangedEvent = value; }
    public static Action<string> LoadNewSceneEvent { get => loadNewSceneEvent; set => loadNewSceneEvent = value; }
    public static Action LoadingDoneEvent { get => loadingDoneEvent; set => loadingDoneEvent = value; }
    public static Action StartAutosaveEvent { get => startAutosaveEvent; set => startAutosaveEvent = value; }

    public static void BroadcastLoadingStartedEvent()
    {
        LoadingStartedEvent?.Invoke();
    }
    public static void BroadcastProgressChangedEvent(Variant progress)
    {
        ProgressChangedEvent?.Invoke(progress);
    }
    public static void BroadcastLoadNewSceneEvent(string scenePath)
    {
        LoadNewSceneEvent?.Invoke(scenePath);
    }
    public static void BroadcastLoadingDoneEvent()
    {
        LoadingDoneEvent?.Invoke();
    }
    public static void BroadcastStartAutoloadEvent()
    {
        StartAutosaveEvent?.Invoke();
    }
    public static void StartLoadingScene(string scenePath)
    {
        try
        {
            Error state = LoadThreadedRequest(scenePath);
            if (state == Error.Ok) 
                BroadcastLoadingStartedEvent();
            else 
                GD.PrintErr(state);
        }
        catch (System.Exception e)
        {
            GD.PrintErr(e.Message);
            throw;
        }
    }
    public static bool LoadProgress(string scenePath)
    {
        try
        {
            ThreadLoadStatus loadStatus = LoadThreadedGetStatus(scenePath, _progress);
            switch ((int)loadStatus)
            {
                case 0:
                case 2:
                    GD.PrintErr(loadStatus);
                    return false;
                case 1:
                    BroadcastProgressChangedEvent(_progress[0]);
                    return true;
                case 3:
                    BroadcastProgressChangedEvent(1.0);
                    BroadcastLoadingDoneEvent();
                    return false;
            }
            return true;
        }
        catch (System.Exception e)
        {
            GD.PrintErr(e.Message);
            throw;
        }
    }
    public static PackedScene GetLoadedScene(string scenePath)
    {
        return (PackedScene)LoadThreadedGet(scenePath);
    }
}