using Godot.Collections;
using Godot;
using Shared.Resources.Exceptions;

namespace Shared.Resources.Data;

/// <summary>
/// This resource is used to store the list of current saves the
/// player has. It uses a dictionary with an int as the key so the
/// user can save in specific slots and the save will remember 
/// which slot it was saved in.
/// </summary>
public partial class SaveDirectoryResource : Resource
{
    [Export] private Dictionary<int, string> directories = [];
    public int DirectoryCount { get => directories.Count; }
    /// <summary>
    /// Creates a new directory entry. Called in the <see cref="Managers.Data.SaveDirectoryManager"/>
    /// whenever the player creates a new save.
    /// </summary>
    /// <param name="slot">The slot the player is saving in.</param>
    /// <param name="directory">The name of the directory being created.</param>
    /// <exception cref="System.Exception">thrown when the slot is already filled.</exception>
    public void AddDirectory(int slot, string directory)
    {
        if(SlotExists(slot))
            throw new SlotAlreadyExistsException($"Attempt to create {directory} in slot {slot} failed. This slot already exists!");
        directories.Add(slot, directory);
    }
    /// <summary>
    /// Deletes an existing directory entry. Called in the <see cref="Managers.Data.SaveDirectoryManager"/>
    /// whenever the player deletes a save.
    /// <param name="slot">The slot the player is deleting.</param>
    /// <exception cref="SlotIsEmptyException">thrown when the slot is already filled.</exception>
    public void RemoveDirectory(int slot)
    {
        if(!SlotExists(slot))
            throw new SlotIsEmptyException($"Attempt to remove the directory in slot {slot} failed. This slot isn't filled!");
        directories.Remove(slot);
    }
    /// <summary>
    /// Returns the directory at the given slot.
    /// </summary>
    /// <param name="slot">The slot to return.</param>
    /// <returns>a string of the directory's name</returns>
    /// <exception cref="SlotIsEmptyException">thrown when the slot is already filled.</exception>
    public string GetDirectory(int slot)
    {
        if(!SlotExists(slot))
            throw new SlotIsEmptyException($"Attempt to get the directory in slot {slot} failed. This slot isn't filled!");
        return directories[slot];
    }
    /// <summary>
    /// Returns the dictionary of all directories the player has saved.
    /// </summary>
    /// <returns>the dictionary of directories.</returns>
    public Dictionary<int, string> GetDirectories()
    {
        return directories;
    }
    public bool SlotExists(int slot)
    {
        return directories.ContainsKey(slot);
    }
}