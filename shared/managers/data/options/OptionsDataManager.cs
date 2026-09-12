using System.Collections.Generic;
using Godot;
using Shared.Resources.Data;

namespace Shared.Managers.Data.Options;

/// <summary>
/// This class is used to manage all the data around options.
/// </summary>
public static class OptionsDataManager
{
    public static readonly string FILE_PATH = $"user://options.json";
    private static OptionsData _data;

    /// <summary>
    /// A dictionary containing all of the audio buses this game uses.
    /// </summary>
    public static Dictionary<string, double> AudioBusVolumes
    {
        get
        {
            if (_data == null) FileManager.ReadFile(FILE_PATH, ref _data);
            return _data.audioBusVolumes;
        }
    }
    /// <summary>
    /// A dictionary containing the full saved input map. Only rebinded controls
    /// are saved here.
    /// </summary>
    public static Dictionary<string, List<InputData>> SavedInputMap
    {
        get
        {
            if (_data == null) FileManager.ReadFile(FILE_PATH, ref _data);
            return _data.savedInputMap;
        }
    }
    /// <summary>
    /// Saves an audio bus and its current volume to the options data
    /// file. This is used to save a new bus to the file.
    /// </summary>
    /// <param name="busName">The name of the bus.</param>
    /// <param name="busVolume">The volume of the bus.</param>
    public static void AddAudioBus(string busName, double busVolume)
    {
        AudioBusVolumes.Add(busName, busVolume);
        FileManager.WriteFile(FILE_PATH, _data);
    }
    /// <summary>
    /// Updates an audio bus's volume if it is currently saved.
    /// </summary>
    /// <param name="busName">The name of the bus.</param>
    /// <param name="busVolume">The volume of the bus.</param>
    public static void UpdateAudioBusVolume(string busName, double busVolume)
    {
        if (AudioBusInDictionary(busName)) AudioBusVolumes[busName] = busVolume;
        FileManager.WriteFile(FILE_PATH, _data);
    }
    /// <summary>
    /// Checks if a bus is currently saved.
    /// </summary>
    /// <param name="busName">The bus to check against the saved buses.</param>
    /// <returns>Returns a boolean for whether the bus in in the dictionary.</returns>
    public static bool AudioBusInDictionary(string busName)
    {
        return AudioBusVolumes.ContainsKey(busName);
    }

    private static void MapInput(string actionName, List<InputData> actions)
    {
        SavedInputMap.Add(actionName, actions);
        FileManager.WriteFile(FILE_PATH, _data);
    }
    private static void UpdateInputMap(string actionName, List<InputData> actions)
    {
        SavedInputMap[actionName] = actions;
        FileManager.WriteFile(FILE_PATH, _data);
    }
    /// <summary>
    /// Checks if an input action is mapped to the <see cref="SavedInputMap"/>
    /// </summary>
    /// <param name="actionName">The action to check against 
    /// the <see cref="SavedInputMap"/></param>
    /// <returns>A boolean of whether the action is saved or not.</returns>
    public static bool InputMapped(string actionName)
    {
        return SavedInputMap.ContainsKey(actionName);
    }
    /// <summary>
    /// Saves an input action to the <see cref="SavedInputMap"/>. It's based 
    /// off the current saved keybinds that the action has.
    /// </summary>
    /// <param name="actionName">The name of the action to save.</param>
    public static void SaveInputToMap(string actionName)
    {
        List<InputData> actions = new();
        foreach (InputEvent action in InputMap.ActionGetEvents(actionName))
        {
            actions.Add(MapInputEventToInputData(action));
        }
        if (InputMapped(actionName))
        {
            UpdateInputMap(actionName, actions);
        }
        else
        {
            MapInput(actionName, actions);
        }
    }

    private static InputData MapInputEventToInputData(InputEvent action)
    {
        return action switch
        {
            InputEventKey eventKey => new InputData(InputType.Keyboard, (int)eventKey.Keycode),
            InputEventMouseButton eventMouse => new InputData(InputType.Mouse, (int)eventMouse.ButtonIndex),
            InputEventJoypadButton eventJoypad => new InputData(InputType.Joypad, (int)eventJoypad.ButtonIndex),
            InputEventJoypadMotion eventAxis => new InputData(
                eventAxis.Axis switch
                {
                    JoyAxis.LeftX => InputType.JoyAxisLeftX,
                    JoyAxis.LeftY => InputType.JoyAxisLeftY,
                    JoyAxis.RightX => InputType.JoyAxisRightX,
                    JoyAxis.RightY => InputType.JoyAxisRightY,
                    JoyAxis.TriggerLeft => InputType.TriggerX,
                    JoyAxis.TriggerRight => InputType.TriggerY,
                    _ => InputType.Invalid,
                },
                ConvertEventAxisValueToInt(eventAxis.AxisValue)
            ),
            _ => new InputData(InputType.Invalid, 0),
        };
    }
    private static int ConvertEventAxisValueToInt(float axisValue)
    {
        if (axisValue > 0) return Mathf.CeilToInt(axisValue);
        else return Mathf.FloorToInt(axisValue);
    }

    /// <summary>
    /// Used to load the keybinds a player has saved for an 
    /// action.
    /// </summary>
    /// <param name="action">The action to attempt to load.</param>
    public static void LoadKeybindsFromSavefile(string action)
    {
        if (!InputMapped(action)) return;

        InputMap.ActionEraseEvents(action);
        foreach (InputData inputData in SavedInputMap[action])
        {
            InputEvent newEvent = MapInputDataToInputEvent(inputData);
            InputMap.ActionAddEvent(action, newEvent);
        }
    }

    private static InputEvent MapInputDataToInputEvent(InputData inputData)
    {
        return inputData.inputType switch
        {
            InputType.Keyboard => new InputEventKey()
            {
                Keycode = (Key)inputData.buttonIndex
            },
            InputType.Mouse => new InputEventMouseButton()
            {
                ButtonIndex = (MouseButton)inputData.buttonIndex
            },
            InputType.Joypad => new InputEventJoypadButton()
            {
                ButtonIndex = (JoyButton)inputData.buttonIndex
            },
            InputType.JoyAxisLeftX => new InputEventJoypadMotion()
            {
                Axis = JoyAxis.LeftX,
                AxisValue = inputData.buttonIndex,
            },
            InputType.JoyAxisLeftY => new InputEventJoypadMotion()
            {
                Axis = JoyAxis.LeftY,
                AxisValue = inputData.buttonIndex,
            },
            InputType.JoyAxisRightX => new InputEventJoypadMotion()
            {
                Axis = JoyAxis.RightX,
                AxisValue = inputData.buttonIndex,
            },
            InputType.JoyAxisRightY => new InputEventJoypadMotion()
            {
                Axis = JoyAxis.RightY,
                AxisValue = inputData.buttonIndex,
            },
            InputType.TriggerX => new InputEventJoypadMotion()
            {
                Axis = JoyAxis.TriggerLeft,
                AxisValue = inputData.buttonIndex,
            },
            InputType.TriggerY => new InputEventJoypadMotion()
            {
                Axis = JoyAxis.TriggerRight,
                AxisValue = inputData.buttonIndex,
            },
            _ => new InputEventKey(),
        };
    }
}