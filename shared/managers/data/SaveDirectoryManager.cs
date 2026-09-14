using System.Collections.Generic;
using System.IO;
using Godot;
using Godot.Collections;
using Shared.Resources.Data;
using Shared.Resources.Exceptions;

namespace Shared.Managers.Data;

/// <summary>
/// This class manages the user's save files, classified here as directories, as
/// every "save file" is actually the name of a folder where the player's data 
/// is stored. Each data type (e.g. player data, object data, enemy data, etc.) 
/// is saved in separate files to decentralize data storage and keep each data 
/// type separate.
/// </summary>
public static class SaveDirectoryManager 
{
    private static readonly string DIR_FILE_PATH = "user://save-info.tres";
    /// <summary>
    /// The maximum amount of saves allowed.
    /// </summary>
    public static readonly int MAX_DIRECTORY_COUNT = 20;

    private static string currentDirectory;
    private static SaveDirectoryResource directoryResource;
    
    /// <summary>
    /// A reference to the current directory the player is saving their game 
    /// in. Has a private setter so we can't change the current directory from
    /// outside this class.
    /// </summary>
    public static string CurrentDirectory 
    { 
        get => currentDirectory; 
        private set => currentDirectory = value;
        
    }
    /// <summary>
    /// A reference to all directories the player has created.
    /// </summary>
    internal static SaveDirectoryResource DirectoryResource
    {
        get
        {
            CheckDirectoryDataExists();
            return directoryResource;
        }
        private set => directoryResource = value;
    }
    /// <summary>
    /// Creates a new directory at the given slot. Currently, we only pass
    /// in a slot and the name is created automatically from that.
    /// </summary>
    /// <param name="slot">the slot we're saving to.</param>
    /// <returns>the name of the dire</returns>
    /// <exception cref="SlotLimitReachedException">thrown when the
    /// player tries to create a save when they've reached the 
    /// directory limit.</exception>
    public static string CreateDirectory(int slot)
    {
        if(SlotLimitReached())
            throw new SlotLimitReachedException("Attempting to start a game when the max save limit has been reached");

        string directoryName = $"user://usr{slot}";
        MakeDirectory(directoryName);
        DirectoryResource.AddDirectory(slot, directoryName);

        ResourceSaver.Save(DirectoryResource, DIR_FILE_PATH);
        return directoryName;
    }
    private static void MakeDirectory(string directory)
    {
        if(DirAccess.DirExistsAbsolute(directory))
            throw new SlotAlreadyExistsException($"Attempt to create directory {directory} failed. This directory already exists!");
        DirAccess.MakeDirAbsolute(directory);
    }
    /// <summary>
    /// Deletes a directory at the given slot.
    /// </summary>
    /// <param name="slot">The slot to delete.</param>
    /// <exception cref="SlotIsEmptyException">thrown when the slot 
    /// is empty.</exception>
    public static void DeleteDirectory(int slot)
    {
        if(!SlotExists(slot))
            throw new SlotIsEmptyException($"Attempt to delete directory at {slot} failed. This slot is not filled!");

        string directoryName = DirectoryResource.GetDirectory(slot);
        RemoveDirectory(directoryName);

        DirectoryResource.RemoveDirectory(slot);
        ResourceSaver.Save(DirectoryResource, DIR_FILE_PATH);
    }
    private static void RemoveDirectory(string directory)
    {
        if(!DirAccess.DirExistsAbsolute(directory))
            throw new System.Exception($"Attempt to remove directory {directory} failed. This directory does not exist!");
        DirAccess.RemoveAbsolute(directory);
    }
    
    /// <summary>
    /// Returns the directory at the given slot.
    /// </summary>
    /// <param name="slot">The slot to grab.</param>
    /// <returns>A string of the directory we're grabbing.</returns>
    public static string GetDirectory(int slot)
    {
        return DirectoryResource.GetDirectory(slot);
    }

    /// <summary>
    /// Copies one save slot into the other, copying all files and folders from one into the
    /// other.
    /// </summary>
    /// <param name="slotFrom">The slot to copy from</param>
    /// <param name="slotTo">The slot to copy to</param>
    /// <exception cref="SlotIsEmptyException">Thrown when the from slot has no data in it 
    /// already.</exception>
    public static void CopyDirectory(int slotFrom, int slotTo)
    {
        if(!SlotExists(slotFrom))
            throw new SlotIsEmptyException($"Attempt to copy directory at slot {slotFrom} failed. This directory does not exist!");

        string fromDirectory = DirectoryResource.GetDirectory(slotFrom);
        string toDirectory = SlotExists(slotTo) ? DirectoryResource.GetDirectory(slotTo) : CreateDirectory(slotTo);

        CopyDirectoryRecursive(fromDirectory, toDirectory);
    }
    private static void CopyDirectoryRecursive(string fromDirectory, string toDirectory)
    {
        var fromFiles = DirAccess.GetFilesAt(fromDirectory);
        var fromDirs = DirAccess.GetDirectoriesAt(fromDirectory);
        
        if(!DirAccess.DirExistsAbsolute(toDirectory))
            DirAccess.MakeDirAbsolute(toDirectory);
        foreach(string fileName in fromFiles)
            DirAccess.CopyAbsolute($"{fromDirectory}/{fileName}", $"{toDirectory}/{fileName}");
        foreach(string dirName in fromDirs)
            CopyDirectoryRecursive($"{fromDirectory}/{dirName}", $"{toDirectory}/{dirName}");
    }
    public static void SetCurrentDirectory(int slot)
    {
        if(!SlotExists(slot))
            throw new System.Exception($"Attempt to set current directory to {slot} failed. This slot is not filled!");
        CurrentDirectory = DirectoryResource.GetDirectory(slot);
    }
    public static void ClearCurrentDirectory()
    {
        CurrentDirectory = null;
    }
    private static bool SlotLimitReached()
    {
        return DirectoryResource.DirectoryCount == MAX_DIRECTORY_COUNT;
    }
    public static bool CurrentDirectoryExists()
    {
        return CurrentDirectory != null;
    }
    public static bool SlotExists(int slot)
    {
        return DirectoryResource.SlotExists(slot);
    }
    private static void CheckDirectoryDataExists()
    {
        directoryResource ??= ResourceLoader.Load<SaveDirectoryResource>(DIR_FILE_PATH) ?? new();
    }
}