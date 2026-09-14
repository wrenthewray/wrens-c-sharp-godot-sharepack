using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Shared.Resources;
/// <summary>
/// A resource that is used to save a list of all the actions defined
/// in the <see cref="InputMap"/> 
/// </summary>

[GlobalClass]
public partial class InputMapResource : Resource
{
    [Export] private Dictionary<string, Array<InputEvent>> actions = new();

    public InputMapResource()
    {
        // get a list of our custom actions and add it to the dictionary.
        foreach (string action in InputMap.GetActions().Where((action) => !((string)action).Contains("ui_")))
            AddAction(action, InputMap.ActionGetEvents(action));
        
    }

    public void AddAction(string action, Array<InputEvent> events)
    {
        if(ContainsAction(action))
            throw new Exception($"Action {action} already exists in the map!");

        actions.Add(action, events);
        SyncThisWithInputMap();
    }
    public Array<InputEvent> GetActionEvents(string action)
    {
        if(!ContainsAction(action)) 
            throw new Exception($"Action {action} doesn't exist in the map!");
        return actions[action];
    }

    public bool ContainsAction(string action)
    {
        return actions.ContainsKey(action);
    }
    public void SwitchActionEvent(string action, InputEvent newEvent)
    {
        if(!ContainsAction(action))
            AddAction(action, [newEvent]);
        
        InputEvent inputEventMatch = actions[action].First((inputEvent) => EventsMatch(newEvent, inputEvent));
        if(inputEventMatch != null)
            ReplaceInputEvent(action, newEvent, inputEventMatch);

        SyncActionWithInputMap(action);
    }

    private static bool EventsMatch(InputEvent newEvent, InputEvent inputEvent)
    {
        return newEvent.GetType().Name == inputEvent.GetType().Name;
    }

    private void ReplaceInputEvent(string action, InputEvent newEvent, InputEvent oldEvent)
    {
        actions[action].Remove(oldEvent);
        actions[action].Add(newEvent);
    }


    private void SyncActionWithInputMap(string action)
    {
        if(!InputMap.HasAction(action))
            throw new Exception($"Godot's input map doesn't have a record of action {action}.");
        InputMap.ActionEraseEvents(action);
        foreach(InputEvent inputEvent in actions[action])
            InputMap.ActionAddEvent(action, inputEvent);
    }
    internal void SyncThisWithInputMap()
    {
        foreach(string action in actions.Keys)
            SyncActionWithInputMap(action);
    }
}