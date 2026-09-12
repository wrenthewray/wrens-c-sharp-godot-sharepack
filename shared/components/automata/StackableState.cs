using System;
using Godot;
using Godot.Collections;
using Shared.Resources.Exceptions;

namespace Shared.Components.Automata;

/// <summary>
/// An abstract class that all states must derive from. When implementing
/// states for a new entity, we should create a base state type from this class 
/// that implements all the methods and uses dependency injection to get any
/// dependencies, which reduces the amount of work it takes to implement a 
/// new state.
/// </summary>
[GlobalClass, Icon("res://addons/at-icons/node/at.svg")]
public abstract partial class StackableState : State
{
    [Signal] public delegate void PushStateEventHandler(string stateName);
    [Signal] public delegate void PopSelfEventHandler(string stateName);

    protected virtual void Push(string stateName)
    {
        EmitSignal(SignalName.PushState, stateName);
    }
    protected virtual void Pop()
    {
        EmitSignal(SignalName.PopSelf, ToString());
    }
    protected override void TransitionTo(string stateName)
    {
        throw new CannotTransitionException("This machine is a pushdown automata. As such, directly transitioning to a state would bypass the state stack, which is forbidden.");
    }
}