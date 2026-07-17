using System;
using Godot;
using Godot.Collections;
using Shared.Models.Exceptions;

namespace Shared.Components.Automata
{
    /// <summary>
    /// An abstract class that all states must derive from. When implementing
    /// states for a new entity, we should create a base state type from this class 
    /// that implements all the methods and uses dependency injection to get any
    /// dependencies, which reduces the amount of work it takes to implement a 
    /// new state.
    /// </summary>
    [GlobalClass, Icon("res://addons/at-icons/node/at.svg")]
    public abstract partial class QueuableState : State
    {
        [Signal] public delegate void EnqueueStateEventHandler(string stateName);
        [Signal] public delegate void DequeueSelfEventHandler(string stateName);

        protected virtual void EnqueueState(string stateName)
        {
            EmitSignal(SignalName.EnqueueState, stateName);
        }
        protected virtual void DequeueSelf()
        {
            EmitSignal(SignalName.DequeueSelf, ToString());
        }
        protected override void TransitionToState(string stateName)
        {
            throw new CannotTransitionException("This machine is a queue automata. As such, directly transitioning to a state would bypass the state queue, which is forbidden.");
        }
    }
}