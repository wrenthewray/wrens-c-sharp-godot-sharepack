namespace Shared.Resources.Exceptions;

/// <summary>
/// An exception that is thrown when a save slot has data when 
/// it shouldnt.
/// </summary>
public class SlotAlreadyExistsException(string message) : System.Exception(message)
{
}