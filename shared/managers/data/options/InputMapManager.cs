using Godot;
using Godot.Collections;
using Shared.Resources;

namespace Shared.Managers.Data.Options;

public static class InputMapManager
{
    private static readonly string FILE_PATH = "user://map.tres";
    private static InputMapResource inputMapResource;

    private static InputMapResource InputMapResource
    {
        get
        {
            CheckInputMapResourceExists();
            return inputMapResource;
        }
        set => inputMapResource = value;
    }

    public static void AddAction(string actionName, Array<InputEvent> events)
    {
        InputMapResource.AddAction(actionName, events);
        SaveInputMapResource();
    }
    public static bool HasAction(string actionName)
    {
        return InputMapResource.ContainsAction(actionName);
    }

    internal static void ChangeActionEvent(string action, InputEvent inputEvent)
    {
        InputMapResource.ChangeActionEvent(action, inputEvent);
        SaveInputMapResource();
    }
    internal static Array<InputEvent> GetActionEvents(string action)
    {
        return InputMapResource.GetActionEvents(action);
    }

    private static void CheckInputMapResourceExists()
    {
        inputMapResource ??= ResourceManager<InputMapResource>.Load(FILE_PATH) ?? new();
    }
    public static void SyncInputMapResource()
    {
        InputMapResource.SyncEventMapWithInputMap();
    }
    internal static void SaveInputMapResource()
    {
        ResourceManager<InputMapResource>.Save(FILE_PATH, InputMapResource);
    }
}