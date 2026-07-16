using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Shared.Components.Automata
{
    [Icon("res://addons/at-icons/node/conveyor_belt.svg")]
    public partial class QueueAutomataComponent<[MustBeVariant] T> : StateMachineComponent<T> where T : State
    {
        protected readonly Queue<T> stateQueue = new();
        /// <summary>
        /// Queues the given state in the state queue.
        /// </summary>
        /// <param name="stateName">The name of the state to queue.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the state can't 
        /// be found in the states array.</exception>
        public virtual void QueueState(string stateName)
        {
            if (!StateNameIsValid(stateName))
                throw new KeyNotFoundException($"{stateName} is not inside the player's action state machine.");
            stateQueue.Enqueue(states[stateName]);
        }
        /// <summary>
        /// Transitions to the next state in the state queue. 
        /// If the state queue is empty, we transition to the 
        /// base state.
        /// </summary>
        public virtual void TransitionToNextState()
        {
            if (StateQueueIsEmpty())
            {
                TransitionToBaseState();
                return;
            }
            if (CurrentStateIs(baseState))
                return;
            TransitionToState(stateQueue.Dequeue());
        }
        protected virtual void TransitionToBaseState()
        {
            if (CurrentStateIs(baseState))
                return;
            TransitionToState(baseState);
        }
        public bool StateQueueIsEmpty()
        {
            return stateQueue.Count == 0;
        }
        public void ClearStateQueue()
        {
            stateQueue.Clear();
        }
    }
}