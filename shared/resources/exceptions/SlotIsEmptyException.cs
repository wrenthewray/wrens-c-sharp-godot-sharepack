namespace Shared.Resources.Exceptions;

/// <summary>
/// An exception that is thrown when a save slot is empty when
/// it shouldn't be.
/// </summary>
public class SlotIsEmptyException(string message) : System.Exception(message)
{
}