using System.Collections.Generic;
using Godot;
using Shared.Entities.Generics;
using Shared.Resources.Exceptions;

namespace Shared.Managers.Pools;

/// <summary>
/// This manager is used to create and cache pools of commonly instantiated 
/// objects. This is done to reduce the amount of processing power used in
/// creating and displaying objects. By saving the objects to a pool, we 
/// save resources on destroying old objects and creating new ones, as when
/// we want another object, we can grab an already existing one from the pool.
/// <br/><br/>
/// C# creates separate instances for static classes with a generic type 
/// that is based on the type <typeparamref name="T"/>, so each separate 
/// type of object needs a script so it may be passed as a typeparam to 
/// this class.
/// </summary>
/// <typeparam name="T">The type of the object being pooled. This
/// should be unique for each object used.</typeparam>
public static class ObjectPoolManager<T> where T : PoolableEntity<T>
{
    /// <summary>
    /// A reference to the scene that represents the object being
    /// pooled.
    /// </summary>
    private static PackedScene objectSceneReference = null;
    private static readonly Stack<T> pool = new();

    /// <summary>
    /// This method pushes a new object to the pool. Whenever a new
    /// object is created by the pool, we attach this method to the 
    /// object's <see cref="Node.TreeExited"/> signal.
    /// </summary>
    /// <param name="obj">The object to add to the pool.</param>
    public static void Push(T obj)
    {
        objectSceneReference ??= ResourceLoader.Load<PackedScene>(obj.SceneFilePath);
        pool.Push(obj);
    }
    /// <summary>
    /// This method grabs an object from the pool. If the pool is
    /// empty, we create a new object from the object's scene reference.
    /// </summary>
    /// <returns>The pooled object ready for use.</returns>
    /// <exception cref="NoObjectSceneReferenceException"></exception>
    public static T Grab()
    {
        if (PoolIsEmpty())
            if (NoObjectSceneReference())
                throw new NoObjectSceneReferenceException($"No reference to scene for object pool of type {nameof(T)}.");
            else
                return objectSceneReference?.Instantiate<T>();
        
        return pool.Pop();
    }
    /// <summary>
    /// Returns true if the pool is empty, false otherwise.
    /// </summary>
    public static bool PoolIsEmpty()
    {
        return pool.Count == 0;
    }

    public static bool NoObjectSceneReference()
    {
        return objectSceneReference == null;
    }

    public static void SetObjectSceneReference(PackedScene objRefScene)
    {
        if (NoObjectSceneReference())
            objectSceneReference = objRefScene;
    }
    public static void SetObjectSceneReference(T objRef)
    {
        if (NoObjectSceneReference())
            objectSceneReference = ResourceLoader.Load<PackedScene>(objRef.SceneFilePath);
    }
    public static void SetObjectSceneReference(string filePath)
    {
        if (NoObjectSceneReference())
            objectSceneReference = ResourceLoader.Load<PackedScene>(filePath);
    }
}