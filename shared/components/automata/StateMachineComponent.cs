using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Shared.Resources.Exceptions;

namespace Shared.Components.Automata;

/// <summary>
/// The base class for all state machines. Contains the basic logic for creating,
/// running, and transitioning between states. This class isn't meant for Godot, 
/// and it requires implementation from other classes to function properly. It's
/// generic so that we can handle multiple state types and combinations. 
/// <br/><br/>
/// Note: the base state for any state machine derived from this class is the first
/// element of the list. 
/// </summary>
/// <typeparam name="T">The type of state the machine is using.</typeparam>
[Icon("res://addons/at-icons/node/recycle.svg")]
public partial class StateMachineComponent<[MustBeVariant] T> : Node where T : State
{
    protected T baseState;
    private T currentState;
    protected virtual T CurrentState
    {
        get => currentState;
        set => currentState = value;
    }
    /// <summary>
    /// Signal that's emitted when initializing the states. 
    /// </summary>
    /// <param name="objects">An array of Variant the states need for initializing .</param>
    [Signal] public delegate void InitEventHandler(Array<GodotObject> objects);
    /// <summary>
    /// The generic array used in all state machines. Replace with
    /// new implementation on each outward facing state machine so 
    /// Godot export typing works properly.
    /// </summary>
    protected virtual Array<T> ExportedStates { get; set; } = new();

    protected readonly System.Collections.Generic.Dictionary<string, T> states = new();

    public override void _Ready()
    {
        foreach (T state in ExportedStates)
        {
            if (baseState == null)
                baseState = state;
            states.Add(state.ToString(), state);
            state.TransitionToState += TransitionToState;
            Init += state.Initialize;
        }
        EmitInit();
        CurrentState = baseState;
        CurrentState.Enter();
    }
    public override void _Process(double delta)
    {
        CurrentState.HandleInput();
        CurrentState.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicsUpdate(delta);
    }
    /// <summary>
    /// Transitions to a new state using the state's name.
    /// </summary>
    /// <param name="stateName">State to transition to.</param>
    /// <exception cref=""></exception>
    internal virtual void TransitionToState(string stateName)
    {
        if (!StateNameIsValid(stateName))
            return;
        TransitionToState(states[stateName]);
    }
    /// <summary>
    /// Transitions to a new state using the state object. This method 
    /// should only be called by derived classes when transitioning.
    /// </summary>
    /// <param name="state">State to transition to.</param>
    /// <exception cref="StateCannotTransitionToSelfException"></exception>
    private protected virtual void TransitionToState(T state)
    {
        if (CurrentStateIs(state))
            throw new StateCannotTransitionToSelfException($"State {state.GetType().Name} is trying to transition to itself!");
        CurrentState.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }

    public bool CurrentStateIs(T state)
    {
        return CurrentState == state;
    }
    public bool CurrentStateIs(string stateName)
    {
        if(!StateNameIsValid(stateName))
            throw new KeyNotFoundException($"The state {stateName} doesn't exist in the list of possible states.");
        return CurrentState == states[stateName];
    }
    public bool CurrentStateIsBaseState()
    {
        return CurrentState == baseState;
    }
    public bool StateNameIsValid(string stateName)
    {
        return states.ContainsKey(stateName);
    }
    protected virtual void EmitInit()
    {
        EmitSignal(SignalName.Init);
    }
}