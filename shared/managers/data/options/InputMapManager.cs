using Godot;
using Godot.Collections;
using Shared.Resources;

namespace Shared.Managers.Data.Options;
/// <summary>
/// This class stores a dictionary of input actions and their associated
/// input events. This is mostly used by option menus to save and load 
/// keybinds.
/// </summary>
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
        InputMapResource.SwitchActionEvent(action, inputEvent);
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
        InputMapResource.SyncThisWithInputMap();
    }
    internal static void SaveInputMapResource()
    {
        ResourceManager<InputMapResource>.Save(FILE_PATH, InputMapResource);
    }
}