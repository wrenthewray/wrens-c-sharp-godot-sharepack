using System;
using Godot;
using Godot.Collections;
using Shared.Managers.Data.Options;
using Shared.Resources.Data;

namespace Shared.Resources;

[GlobalClass]
public partial class InputEventMap : Resource
{
    [Export] private Dictionary<string, Array<InputEvent>> actions = new();

    public void AddAction(string action, Array<InputEvent> events)
    {
        if(ContainsAction(action))
            throw new Exception($"Action {action} already exists in the map!");
        actions.Add(action, events);
        SyncEventMapWithInputMap();
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
    public void ChangeActionEvent(string action, InputEvent newEvent)
    {
        if(!ContainsAction(action))
            AddAction(action, [newEvent]);
        foreach(InputEvent inputEvent in actions[action])
        {
            if(newEvent.GetType().Name == inputEvent.GetType().Name)
            {
                actions[action].Remove(inputEvent);
                actions[action].Add(newEvent);
                break;
            }
        }
        SyncActionWithInputMap(action);
    }
    private void SyncActionWithInputMap(string action)
    {
        if(!InputMap.HasAction(action))
            throw new Exception($"Godot's input map doesn't have a record of action {action}.");
        InputMap.ActionEraseEvents(action);
        foreach(InputEvent inputEvent in actions[action])
            InputMap.ActionAddEvent(action, inputEvent);
    }
    internal void SyncEventMapWithInputMap()
    {
        foreach(string action in actions.Keys)
            SyncActionWithInputMap(action);
    }
}