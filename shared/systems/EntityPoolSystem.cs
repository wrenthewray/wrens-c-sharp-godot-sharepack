using System.Collections.Generic;
using Godot;
using Shared.Entities.Generics;
using Shared.Resources.Exceptions;

namespace Shared.Systems;

/// <summary>
/// This system is used to create and cache pools of commonly instantiated 
/// entities. This is done to reduce the amount of processing power used in
/// creating and displaying entities. By saving the entities to a pool, we 
/// save resources on destroying old entities and creating new ones, as when
/// we want another object, we can grab an already existing one from the pool.
/// <br/><br/>
/// C# creates separate instances for static classes with a generic type 
/// that is based on the type <typeparamref name="T"/>, so each separate 
/// type of object needs a script so it may be passed as a typeparam to 
/// this class.
/// </summary>
/// <typeparam name="T">The type of the object being pooled. This
/// should be unique for each object used.</typeparam>
public static class EntityPoolSystem<T> where T : PoolableEntity<T>
{
    /// <summary>
    /// A reference to the scene that represents the object being
    /// pooled.
    /// </summary>
    private static PackedScene entitySceneReference = null;
    private static readonly Stack<T> pool = new();

    /// <summary>
    /// This method pushes a new object to the pool. Whenever a new
    /// object is created by the pool, we attach this method to the 
    /// object's <see cref="Node.TreeExited"/> signal.
    /// </summary>
    /// <param name="obj">The object to add to the pool.</param>
    public static void Push(T obj)
    {
        entitySceneReference ??= ResourceLoader.Load<PackedScene>(obj.SceneFilePath);
        pool.Push(obj);
    }
    /// <summary>
    /// This method grabs an object from the pool. If the pool is
    /// empty, we create a new object from the object's scene reference.
    /// </summary>
    /// <returns>The pooled object ready for use.</returns>
    /// <exception cref="NoEntitySceneReferenceException"></exception>
    public static T Grab()
    {
        if (PoolIsEmpty())
            if (NoEntitySceneReference())
                throw new NoEntitySceneReferenceException($"No reference to scene for object pool of type {nameof(T)}.");
            else
                return entitySceneReference?.Instantiate<T>();
        
        return pool.Pop();
    }
    /// <summary>
    /// Returns true if the pool is empty, false otherwise.
    /// </summary>
    public static bool PoolIsEmpty()
    {
        return pool.Count == 0;
    }

    public static bool NoEntitySceneReference()
    {
        return entitySceneReference == null;
    }

    public static void SetEntitySceneReference(PackedScene objRefScene)
    {
        if (NoEntitySceneReference())
            entitySceneReference = objRefScene;
    }
    public static void SetEntitySceneReference(T objRef)
    {
        if (NoEntitySceneReference())
            entitySceneReference = ResourceLoader.Load<PackedScene>(objRef.SceneFilePath);
    }
    public static void SetEntitySceneReference(string filePath)
    {
        if (NoEntitySceneReference())
            entitySceneReference = ResourceLoader.Load<PackedScene>(filePath);
    }
}