using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Shared.Resources.Exceptions;

namespace Shared.Components.Automata;

[Icon("res://addons/at-icons/node/conveyor_belt.svg")]
public abstract partial class QueueAutomataComponent<[MustBeVariant] T> : StateMachineComponent<T> where T : QueuableState
{
    protected readonly Queue<T> stateQueue = new();
    public override void _Ready()
    {
        foreach (T state in ExportedStates)
        {
            if (baseState == null)
                baseState = state;
            states.Add(state.ToString(), state);
            state.EnqueueState += Enqueue;
            state.DequeueSelf += DequeueFromState;
            BindStateDependencies += state.BindDependencies;
        }
        EmitBindStateDependencies();
        CurrentState = baseState;
        CurrentState.Enter();
    }
    /// <summary>
    /// Queues the given state in the state queue.
    /// </summary>
    /// <param name="stateName">The name of the state to queue.</param>
    /// <exception cref="KeyNotFoundException">Thrown if the state can't 
    /// be found in the states array.</exception>
    public virtual void Enqueue(string stateName)
    {
        if (!StateNameIsValid(stateName))
            throw new KeyNotFoundException($"{stateName} is not inside the list of possible states.");
        stateQueue.Enqueue(states[stateName]);
    }
    /// <summary>
    /// Transitions to the next state in the state queue. 
    /// If the state queue is empty, we transition to the 
    /// base state.
    /// </summary>
    public virtual void Dequeue()
    {
        if (QueueIsEmpty())
            TransitionToBaseState();
        if (CurrentStateIsBaseState())
            return;
        TransitionToState(stateQueue.Dequeue());
    }
    protected virtual void DequeueFromState(string stateName)
    {
        if(!CurrentStateIs(stateName))
            throw new CannotTransitionException($"State {stateName} attempted to dequeue when it wasn't the current state. This is forbidden.");
        Dequeue();
    }
    protected virtual void TransitionToBaseState()
    {
        if (CurrentStateIsBaseState())
            return;
        TransitionToState(baseState);
    }
    public bool QueueIsEmpty()
    {
        return stateQueue.Count == 0;
    }
    public void Clear()
    {
        stateQueue.Clear();
    }
    internal override void TransitionToState(string stateName)
    {
        throw new CannotTransitionException("This machine is a queue automata. As such, directly transitioning to a state would bypass the state queue, which is forbidden.");
    }
}