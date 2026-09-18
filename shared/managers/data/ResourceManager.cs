using Godot;
using System.Threading.Tasks;

namespace Shared.Managers.Data;

/// <summary>
/// This manager is used to save and load resources of type <typeparamref name="T"/>. 
/// It can save and load resources to a specific file path, or to the current save directory. 
/// It can also load resources in the background using the <see cref="BackgroundLoadManager{T}"/> class.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ResourceManager<T> where T: Resource
{
    /// <summary>
    /// Saves the resource of type <typeparamref name="T"/> to the specified file path.
    /// It uses Godot's <see cref="ResourceSaver"/> class to save the resource.
    /// </summary>
    /// <param name="filePath">The file path to save the resource to.</param>
    /// <param name="data">The resource data to save.</param>
    public static void Save(string filePath, T data)
    {
        ResourceSaver.Save(data, filePath);
    }
    /// <summary>
    /// Saves the resource of type <typeparamref name="T"/> to the specified file name 
    /// and slot. It uses Godot's <see cref="ResourceSaver"/> class to save the resource.
    /// </summary>
    /// <param name="fileName">The name of the file to save the resource to.</param>
    /// <param name="slot">The slot to save the resource to.</param>
    /// <param name="data">The resource data to save.</param>
    public static void Save(string fileName, int slot, T data)
    {
        ResourceSaver.Save(data, $"{SaveDirectoryManager.GetDirectory(slot)}/{fileName}");
    }

    /// <summary>
    /// Attempts to load the given resource of type <typeparamref name="T"/> from 
    /// the specified file path.
    /// </summary>
    /// <param name="filePath">The file path to load from.</param>
    /// <param name="flags">Used to specify whether to use the <see cref="BackgroundLoadManager<T>"/>
    /// class. </param>
    /// <returns>The loaded resource.</returns>
    public static T Load(string filePath, LoadFlags flags = LoadFlags.BACKGROUND)
    {
        return _Load(filePath, flags);
    }
    /// <summary>
    /// Attempts to load the given resource of type <typeparamref name="T"/> from 
    /// the specified file name and slot.
    /// </summary>
    /// <param name="fileName">The file name to load from.</param>
    /// <param name="slot"></param>
    /// <param name="flags">Used to specify whether to use the <see cref="BackgroundLoadManager<T>"/>
    /// class. </param>
    /// <returns>The loaded resource.</returns>
    public static T Load(string fileName, int slot, LoadFlags flags = LoadFlags.BACKGROUND)
    {
        return _Load($"{SaveDirectoryManager.GetDirectory(slot)}/{fileName}", flags);
    }
    /// <summary>
    /// Attempts to load the given resource of type <typeparamref name="T"/> from 
    /// the specified file name and current directory.
    /// </summary>
    /// <param name="fileName">The file name to load from.</param>
    /// <param name="flags">Used to specify whether to use the <see cref="BackgroundLoadManager<T>"/>
    /// class. </param>
    /// <returns>The loaded resource.</returns>
    public static T LoadFromCurrentDirectory(string fileName, LoadFlags flags = LoadFlags.BACKGROUND)
    {
        return _Load($"{SaveDirectoryManager.CurrentDirectory}/{fileName}", flags);
    }
    private static T _Load(string filePath, LoadFlags flags)
    {
        if(flags == LoadFlags.FOREGROUND)
            return ResourceLoader.Load<T>(filePath);
        return BackgroundLoadManager<T>.LoadResource(filePath);
    }

    /// <summary>
    /// Attempts to load the given resource of type <typeparamref name="T"/> from 
    /// the specified file path.
    /// </summary>
    /// <param name="filePath">The file path to load from.</param>
    /// <param name="flags">Used to specify whether to use the <see cref="BackgroundLoadManager<T>"/>
    /// class. </param>
    /// <returns>The loaded resource.</returns>
    public async static Task<T> LoadAsync(string filePath, LoadFlags flags = LoadFlags.BACKGROUND)
    {
        return await _LoadAsync(filePath, flags);
    }
    /// <summary>
    /// Attempts to load the given resource of type <typeparamref name="T"/> from 
    /// the specified file name and slot.
    /// </summary>
    /// <param name="fileName">The file name to load from.</param>
    /// <param name="slot"></param>
    /// <param name="flags">Used to specify whether to use the <see cref="BackgroundLoadManager<T>"/>
    /// class. </param>
    /// <returns>The loaded resource.</returns>
     public static async Task<T> LoadAsync(string fileName, int slot, LoadFlags flags = LoadFlags.BACKGROUND)
    {
        return await _LoadAsync($"{SaveDirectoryManager.GetDirectory(slot)}/{fileName}", flags);
    }
    /// <summary>
    /// Attempts to load the given resource of type <typeparamref name="T"/> from 
    /// the specified file name and current directory.
    /// </summary>
    /// <param name="fileName">The file name to load from.</param>
    /// <param name="flags">Used to specify whether to use the <see cref="BackgroundLoadManager<T>"/>
    /// class. </param>
    /// <returns>The loaded resource.</returns>
    public static async Task<T> LoadFromCurrentDirectoryAsync(string fileName, LoadFlags flags = LoadFlags.BACKGROUND)
    {
        return await _LoadAsync($"{SaveDirectoryManager.CurrentDirectory}/{fileName}", flags);
    }
    private async static Task<T> _LoadAsync(string filePath, LoadFlags flags)
    {
        if(flags == LoadFlags.FOREGROUND)
            return ResourceLoader.Load<T>(filePath);
        return await BackgroundLoadManager<T>.LoadResourceAsync(filePath);
    }
}
public enum LoadFlags
{
    BACKGROUND,
    FOREGROUND
}