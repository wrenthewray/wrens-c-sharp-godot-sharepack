using Godot;
using Godot.Collections;

namespace Shared.Resources.Data;

/// <summary>
/// A class that defines the data to be saved by the <see cref="Managers.Data.Options.OptionsDataManager"/>
/// </summary>
public partial class AudioResource : Resource
{
    [Export] public Dictionary<string, double> audioBusVolumes;
}