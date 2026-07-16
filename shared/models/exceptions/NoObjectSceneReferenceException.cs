using System;

namespace Shared.Models.Exceptions
{
    /// <summary>
    /// An exception thrown when there is no reference to an object's
    /// scene when such a reference is necessary.
    /// </summary>
    /// <param name="message"></param>
    public class NoObjectSceneReferenceException(string message) : Exception(message)
    {
    }
}