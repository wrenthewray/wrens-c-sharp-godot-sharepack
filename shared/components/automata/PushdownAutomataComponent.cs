using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Shared.Components.Automata
{
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
    public partial class PushdownAutomataComponent<[MustBeVariant] T> : StateMachineComponent<T> where T : State
    {
        protected readonly Stack<T> stateStack = new();

        public override void _Ready()
        {
            base._Ready();
            stateStack.Push(baseState);
        }
        public void PushState(string stateName)
        {
            if (!StateNameIsValid(stateName))
                throw new KeyNotFoundException($"{stateName} is not inside the player's body state machine.");
            stateStack.Push(states[stateName]);
            TransitionToState(stateStack.Peek());
        }
        public void PopState()
        {
            if (CurrentStateIs(baseState))
                return;
            stateStack.Pop();
            TransitionToState(stateStack.Peek());
        }
    }
}