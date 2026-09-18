using System;
using Godot;

namespace Shared.Buses;
/// <summary>
/// This class stores events used by the load screen entity and manager to 
/// communicate with each other.
/// </summary>
public static class LoadSceneBus
{
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
}