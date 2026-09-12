using System;
using Godot;
using Godot.Collections;

namespace Shared.Components.Automata;

/// <summary>
/// An abstract class that all states must derive from. When implementing
/// states for a new entity, we should create a base state type from this class 
/// that implements all the methods and uses dependency injection to get any
/// dependencies, which reduces the amount of work it takes to implement a 
/// new state.
/// </summary>
[GlobalClass, Icon("res://addons/at-icons/node/at.svg")]
public abstract partial class State : Resource
{
    [Signal] public delegate void TransitionToStateEventHandler(string stateName);
    /// <summary>
    /// Called when the state is entered.
    /// </summary>
    public abstract void Enter();
    /// <summary>
    /// Called when the state is exited.
    /// </summary>
    public abstract void Exit();
    /// <summary>
    /// Called every frame while this is the active state.
    /// </summary>
    public abstract void Update(double delta);
    /// <summary>
    /// Called every frame while this is the active state. Use 
    /// to handle physics interactions
    /// </summary>
    public abstract void PhysicsUpdate(double delta);
    /// <summary>
    /// Called every frame while this is the active state. Use to
    /// handle input from user.
    /// </summary>
    public abstract void HandleInput();

    public override string ToString()
    {
        return GetType().Name;
    }
    // Custom state comparison functions
    public override bool Equals(object obj)
    {
        var other = obj as State;
        if (!base.Equals(other))
        {
            return ToString().Equals(other?.ToString());
        }
        return false;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(ToString());
    }
    public static bool operator ==(State left, State right)
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }
    public static bool operator !=(State left, State right)
    {
        if (left is null)
            return right is not null;
        return !left.Equals(right);
    }
    /// <summary>
    /// This class is used to initialize a state's dependencies. The
    /// specific dependencies will change depending on the base state's
    /// needs, so we need to implement this on each base state to meet 
    /// their specific requirements.
    /// </summary>
    /// <param name="dependencies">An array of godot objects to pass to the
    /// states during dependency injection.</param>
    public abstract void Initialize(Array<GodotObject> dependencies);
    protected virtual void TransitionTo(string stateName)
    {
        EmitSignal(SignalName.TransitionToState, stateName);
    }
}