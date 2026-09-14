using Godot;
using Shared.Managers.Pools;

namespace Shared.Entities.Generics;

/// <summary>
/// This class is used to represent an object that is pooled. Pooled
/// objects are objects that we create regularly, and so we want to 
/// create them once and save them in a pool when they aren't being
/// shown instead of deleting them, saving processing power. 
/// <br/><br/>
/// Each poolable entity must inherit from this class and pass itself
/// as the typeparam <typeparamref name="T"/> allows the manager to 
/// create a new pool for it.
/// </summary>
public abstract partial class PoolableEntity<T> : Node where T : PoolableEntity<T>
{
    override public void _EnterTree()
    {
        base._EnterTree();
        ObjectPoolManager<T>.SetObjectSceneReference(SceneFilePath);
        TreeExited += PushSelf;
    }

    override public void _ExitTree()
    {
        base._ExitTree();
        TreeExited -= PushSelf;
    }

    internal virtual void PushSelf()
    {
        ObjectPoolManager<T>.Push((T)this);
    }
}