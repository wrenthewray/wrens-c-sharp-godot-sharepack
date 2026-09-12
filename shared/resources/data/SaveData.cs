using Godot;

namespace Shared.Resources.Data;

public partial class SaveData : Resource
{
    [Export] public string name;
    [Export] public int slot;
    [Export] public Location location;
    public SaveData() { }
    public SaveData(string name, int slot)
    {
        this.name = name;
        this.slot = slot;
    }
}