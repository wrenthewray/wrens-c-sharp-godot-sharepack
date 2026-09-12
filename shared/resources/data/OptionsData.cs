using System.Collections.Generic;
using Godot;

namespace Shared.Resources.Data;

/// <summary>
/// A class that defines the data to be saved by the <see cref="OptionsDataManager"/>
/// </summary>
public partial class OptionsData : Resource
{
    public Dictionary<string, double> audioBusVolumes;
    public InputEventMap inputEventMap;

    public OptionsData()
    {
        audioBusVolumes = new();
        inputEventMap = new();
    }
}