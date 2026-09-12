using System.Threading.Tasks;
using Godot;
using static Godot.ResourceLoader;
using Array = Godot.Collections.Array;

namespace Shared.Managers.Data;

public static class BackgroundLoadingManager
{
    public static PackedScene LoadResource(string scenePath)
    {
        if(HasCached(scenePath))
            return (PackedScene)LoadThreadedGet(scenePath);
        return LoadResourceRecursive(scenePath);
    }

    private static PackedScene LoadResourceRecursive(string scenePath)
    {
        ThreadLoadStatus loadStatus = LoadThreadedGetStatus(scenePath);
        switch ((int)loadStatus)
        {
            case 2:
                // error in creation
                throw new System.Exception(nameof(loadStatus));
            case 0:
                Error state = LoadThreadedRequest(scenePath);
                if (state == Error.Ok)
                    goto case 1;
                else
                    throw new System.Exception(nameof(state));
            case 1:
                return LoadResourceRecursive(scenePath);
            case 3:
            default:
                return (PackedScene)LoadThreadedGet(scenePath);
        }
    }

    public static async Task<PackedScene> LoadResourceAsync(string scenePath)
    {
        if(HasCached(scenePath))
            return (PackedScene)LoadThreadedGet(scenePath);
        return await LoadResourceRecursiveAsync(scenePath);
    }
    private static async Task<PackedScene> LoadResourceRecursiveAsync(string scenePath)
    {
        ThreadLoadStatus loadStatus = LoadThreadedGetStatus(scenePath);
        switch ((int)loadStatus)
        {
            case 2:
                // error in creation
                throw new System.Exception(nameof(loadStatus));
            case 0:
                Error state = LoadThreadedRequest(scenePath);
                if (state == Error.Ok)
                    goto case 1;
                else
                    throw new System.Exception(nameof(state));
            case 1:
                return await LoadResourceRecursiveAsync(scenePath);
            case 3:
            default:
                return (PackedScene)LoadThreadedGet(scenePath);
        }
    }
}