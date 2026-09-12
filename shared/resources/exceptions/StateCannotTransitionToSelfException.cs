namespace Shared.Resources.Exceptions;

/// <summary>
/// An exception that is thrown when a state machine tries to transition to the same state it's currently in.
/// </summary>
public class StateCannotTransitionToSelfException(string message) : System.Exception(message)
{
}