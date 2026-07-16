using System.Collections.Generic;
using Godot;
using Shared.Models.Exceptions;

namespace Shared.Managers.Pools
{
    /// <summary>
    /// This manager is used to create and cache pools of commonly
    /// instantiated objects. It caches pools based on the type 
    /// <typeparamref name="T"/>, so its important to make sure 
    /// each different object type being pooled is unique. Each 
    /// pool is represented using a Stack data structure. 
    /// </summary>
    /// <typeparam name="T">The type of the object being pooled. This
    /// should be unique for each object used.</typeparam>
    public static class ObjectPoolManager<T> where T : Node
    {
        /// <summary>
        /// A reference to the scene that represents the object being
        /// pooled.
        /// </summary>
        private static PackedScene objectSceneReference = null;
        private static readonly Stack<T> pool = new();

        /// <summary>
        /// This method pushes a new object to the pool. Whenever a new
        /// object is created by the pool when the pool is empty, we 
        /// attach this method to the object's "TreeExited" signal.
        /// </summary>
        /// <param name="obj">The object to add to the pool.</param>
        private static void Push(T obj)
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
            {
                if (NoObjectSceneReference())
                    throw new NoObjectSceneReferenceException($"No reference to scene for object pool of type {nameof(T)}.");
                T newObject = objectSceneReference?.Instantiate<T>();
                newObject.TreeExited += () => Push(newObject);
                return newObject;
            }
            return pool.Pop();
        }

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
}