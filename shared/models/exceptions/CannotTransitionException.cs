namespace Shared.Models.Exceptions
{
    /// <summary>
    /// An exception that is thrown when a state machine attempts a transition that isn't
    /// possible. 
    /// </summary>
    public class CannotTransitionException(string message) : System.Exception(message)
    {
    }
}