using Godot.Collections;
using Godot;

namespace Shared.Resources.Data;

public partial class SaveDirectoryData : Resource
{
    [Export] private Dictionary<int, string> directories = [];
    public int DirectoryCount { get => directories.Count; }
    public void AddDirectory(int slot, string directory)
    {
        if(SlotExists(slot))
            throw new System.Exception($"Attempt to create {directory} in slot {slot} failed. One or both values already exist!");
        directories.Add(slot, directory);
    }
    public void RemoveDirectory(int slot)
    {
        if(!SlotExists(slot))
            throw new System.Exception($"Attempt to remove the directory in slot {slot} failed. This slot isn't filled!");
        directories.Remove(slot);
    }
    public string GetDirectory(int slot)
    {
        if(!SlotExists(slot))
            throw new System.Exception($"Attempt to get the directory in slot {slot} failed. This slot isn't filled!");
        return directories[slot];
    }
    public Dictionary<int, string> GetDirectories()
    {
        return directories;
    }
    public bool SlotExists(int slot)
    {
        return directories.ContainsKey(slot);
    }
}