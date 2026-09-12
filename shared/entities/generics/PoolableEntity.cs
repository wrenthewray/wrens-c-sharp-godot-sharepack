using Godot;
using Shared.Managers.Pools;

namespace Shared.Entities.Generics;

/// <summary>
/// This class is used to represent an object that can be pooled.
/// </summary>
public abstract partial class PoolableEntity<T> : Node where T : PoolableEntity<T>
{
    override public void _EnterTree()
    {
        base._EnterTree();
        ObjectPoolManager<T>.SetObjectSceneReference(SceneFilePath);;
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