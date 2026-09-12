using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Shared.Resources.Exceptions;

namespace Shared.Components.Automata;

/// <summary>
/// A generic implementation of a Pushdown Automata type state machine. Rather
/// than having states transition between each other, states are pushed and popped
/// from a stack. The top of the stack is the current state, and states have to be 
/// pushed or popped from the stack. 
/// <br/><br/>
/// The reason that we're doing this is to simplify state interactions between each 
/// other. By making the active state the top of the stack, we just need to push or 
/// pop a state from the stack, which greatly simplifies the work a state or designer 
/// needs to do to transition between states.
/// </summary>
/// <typeparam name="T">The type of state the machine is implementing. This type has 
/// to derive from the <see cref="State"/> class.</typeparam>
[Icon("res://addons/at-icons/node/arrow_down_to_bracket.svg")]
public partial class PushdownAutomataComponent<[MustBeVariant] T> : StateMachineComponent<T> where T : StackableState
{
    protected readonly Stack<T> stateStack = new();

    public override void _Ready()
    {
        foreach (T state in ExportedStates)
        {
            if (baseState == null)
                baseState = state;
            states.Add(state.ToString(), state);
            state.PushState += Push;
            Init += state.Initialize;
        }
        EmitInit();
        CurrentState = baseState;
        CurrentState.Enter();
        stateStack.Push(baseState);
    }
    /// <summary>
    /// Pushes a state onto the state stack. Transitions to
    /// that state.
    /// </summary>
    /// <param name="stateName">The name of the state to push.
    /// onto the stack.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the 
    /// given state name doesn't exist in the list of possible 
    /// states.</exception>
    public void Push(string stateName)
    {
        if (!StateNameIsValid(stateName))
            throw new KeyNotFoundException($"{stateName} is not inside the list of possible states.");
        stateStack.Push(states[stateName]);
        TransitionToState(stateStack.Peek());
    }
    /// <summary>
    /// Pops the current state from the stack and transitions
    /// to the next state on the stack.  
    /// </summary>
    public void Pop()
    {
        if (CurrentStateIsBaseState())
            return;
        stateStack.Pop();
        TransitionToState(stateStack.Peek());
    }
    protected void PopFromState(string stateName)
    {
        if(!CurrentStateIs(stateName))
            throw new CannotTransitionException($"The state {stateName} attempted to pop the stack when it wasn't on the top of the stack.");
        Pop();
    }
    internal override void TransitionToState(string stateName)
    {
        throw new CannotTransitionException("This machine is a pushdown automata. As such, directly transitioning to a state would bypass the state stack, which is forbidden.");
    }
}