using Godot;
using System.Threading.Tasks;
using static Godot.ResourceLoader;

namespace Shared.Managers;

/// <summary>
/// This class is used to load resources in the background using Godot's <see cref="LoadThreadedRequest"/> 
/// method. It has an async option as well.
/// </summary>
/// <typeparam name="T">The type of resource being loaded.</typeparam>
public static class BackgroundLoadManager<T> where T : Resource
{
    /// <summary>
    /// Used to load a resource in the background using <see cref="LoadThreadedRequest"/> and
    /// a recursive method that calls itself until the resource is loaded or an error
    /// is thrown. Recursion was chosen to allow us to keep checking for load status without
    /// using a loop by using the recursive method as a loop.
    /// </summary>
    /// <param name="resourceFilePath">The file path we're trying to load from.</param>
    /// <returns>The loaded resource.</returns>
    public static T LoadResource(string resourceFilePath)
    {
        if(HasCached(resourceFilePath))
            return (T) Load(resourceFilePath);
        return LoadResourceRecursive(resourceFilePath);
    }

    private static T LoadResourceRecursive(string resourceFilePath)
    {
        ThreadLoadStatus loadStatus = LoadThreadedGetStatus(resourceFilePath);
        switch (loadStatus)
        {
            case ThreadLoadStatus.Failed:
                // error in creation
                throw new System.Exception(nameof(loadStatus));
            case ThreadLoadStatus.InvalidResource:
                Error state = LoadThreadedRequest(resourceFilePath);
                if (state == Error.Ok)
                    goto case ThreadLoadStatus.InProgress;
                else
                    throw new System.Exception(nameof(state));
            case ThreadLoadStatus.InProgress:
                return LoadResourceRecursive(resourceFilePath);
            case ThreadLoadStatus.Loaded:
            default:
                return (T)LoadThreadedGet(resourceFilePath);
        }
    }

    /// <summary>
    /// Used to load a resource in the background using <see cref="LoadThreadedRequest"/> and
    /// a recursive method that calls itself until the resource is loaded or an error
    /// is thrown. Recursion was chosen to allow us to keep checking for load status without
    /// using a loop by using the recursive method as a loop.
    /// <br/><br/>
    /// This method is async, and generally recommended over the other version.
    /// </summary>
    /// <param name="resourceFilePath">The file path we're trying to load from.</param>
    /// <returns>The loaded resource.</returns>
    public static async Task<T> LoadResourceAsync(string resourceFilePath)
    {
        if(HasCached(resourceFilePath))
            return (T)Load(resourceFilePath);
        return await LoadResourceRecursiveAsync(resourceFilePath);
    }
    private static async Task<T> LoadResourceRecursiveAsync(string resourceFilePath)
    {
        ThreadLoadStatus loadStatus = LoadThreadedGetStatus(resourceFilePath);
        switch (loadStatus)
        {
            case ThreadLoadStatus.Failed:
                // error in creation
                throw new System.Exception(nameof(loadStatus));
            case ThreadLoadStatus.InvalidResource:
                Error state = LoadThreadedRequest(resourceFilePath);
                if (state == Error.Ok)
                    goto case ThreadLoadStatus.InProgress;
                else
                    throw new System.Exception(nameof(state));
            case ThreadLoadStatus.InProgress:
                return await LoadResourceRecursiveAsync(resourceFilePath);
            case ThreadLoadStatus.Loaded:
            default:
                return (T)LoadThreadedGet(resourceFilePath);
        }
    }
}