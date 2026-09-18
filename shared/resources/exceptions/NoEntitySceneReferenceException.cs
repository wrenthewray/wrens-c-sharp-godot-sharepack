using System;

namespace Shared.Resources.Exceptions;

/// <summary>
/// An exception thrown when there is no reference to an entity's
/// scene when such a reference is necessary.
/// </summary>
/// <param name="message"></param>
public class NoEntitySceneReferenceException(string message) : Exception(message)
{
}