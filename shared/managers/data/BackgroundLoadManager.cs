using System.Threading.Tasks;
using Godot;
using static Godot.ResourceLoader;

namespace Shared.Managers.Data;

public static class BackgroundLoadManager<T> where T : Resource
{
    public static T LoadResource(string scenePath)
    {
        if(HasCached(scenePath))
            return (T) Load(scenePath);
        return LoadResourceRecursive(scenePath);
    }

    private static T LoadResourceRecursive(string scenePath)
    {
        ThreadLoadStatus loadStatus = LoadThreadedGetStatus(scenePath);
        switch (loadStatus)
        {
            case ThreadLoadStatus.Failed:
                // error in creation
                throw new System.Exception(nameof(loadStatus));
            case ThreadLoadStatus.InvalidResource:
                Error state = LoadThreadedRequest(scenePath);
                if (state == Error.Ok)
                    goto case ThreadLoadStatus.InProgress;
                else
                    throw new System.Exception(nameof(state));
            case ThreadLoadStatus.InProgress:
                return LoadResourceRecursive(scenePath);
            case ThreadLoadStatus.Loaded:
            default:
                return (T)LoadThreadedGet(scenePath);
        }
    }

    public static async Task<T> LoadResourceAsync(string scenePath)
    {
        if(HasCached(scenePath))
            return (T)Load(scenePath);
        return await LoadResourceRecursiveAsync(scenePath);
    }
    private static async Task<T> LoadResourceRecursiveAsync(string scenePath)
    {
        ThreadLoadStatus loadStatus = LoadThreadedGetStatus(scenePath);
        switch (loadStatus)
        {
            case ThreadLoadStatus.Failed:
                // error in creation
                throw new System.Exception(nameof(loadStatus));
            case ThreadLoadStatus.InvalidResource:
                Error state = LoadThreadedRequest(scenePath);
                if (state == Error.Ok)
                    goto case ThreadLoadStatus.InProgress;
                else
                    throw new System.Exception(nameof(state));
            case ThreadLoadStatus.InProgress:
                return await LoadResourceRecursiveAsync(scenePath);
            case ThreadLoadStatus.Loaded:
            default:
                return (T)LoadThreadedGet(scenePath);
        }
    }
}