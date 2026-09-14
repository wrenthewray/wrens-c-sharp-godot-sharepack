using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Shared.Buses;
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

    
    public static void StartLoadingScene(string scenePath)
    {
        Error state = LoadThreadedRequest(scenePath);
        if (state == Error.Ok) 
            LoadScreenBus.BroadcastLoadingStartedEvent();
        else 
            GD.PrintErr(state);
    }
    public static bool LoadProgress(string scenePath)
    {
        ThreadLoadStatus loadStatus = LoadThreadedGetStatus(scenePath, _progress);
        switch (loadStatus)
        {
            case ThreadLoadStatus.InvalidResource:
            case ThreadLoadStatus.Failed:
                GD.PrintErr(loadStatus);
                return false;
            case ThreadLoadStatus.InProgress:
                LoadScreenBus.BroadcastProgressChangedEvent(_progress[0]);
                return true;
            case ThreadLoadStatus.Loaded:
                LoadScreenBus.BroadcastProgressChangedEvent(1.0);
                LoadScreenBus.BroadcastLoadingDoneEvent();
                return false;
        }
        return true;
    }
    public static PackedScene GetLoadedScene(string scenePath)
    {
        return (PackedScene)LoadThreadedGet(scenePath);
    }
}