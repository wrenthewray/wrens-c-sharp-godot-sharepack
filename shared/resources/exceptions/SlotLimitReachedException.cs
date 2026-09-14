namespace Shared.Resources.Exceptions;

/// <summary>
/// An exception that is thrown when the player attempts to save past
/// the max slot limit.
/// </summary>
public class SlotLimitReachedException(string message) : System.Exception(message)
{
}