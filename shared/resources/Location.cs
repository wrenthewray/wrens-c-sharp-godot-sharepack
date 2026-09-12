using Godot;

namespace Shared.Resources;

/// <summary>
/// This class is used to define location data. It stores a file 
/// path to a scene and a Vector3 that represents the location as
/// a point in that scene.
/// </summary>
public class Location
{
    /// <summary>
    /// The filepath to the scene that the location is in.
    /// </summary>
    public string scenePath;
    /// <summary>
    /// The vector that represents the location's position in
    /// the scene.
    /// </summary>
    public Vector3 position;

    public Location() { }
    public Location(string scenePath, Vector3 position)
    {
        this.scenePath = scenePath;
        this.position = position;
    }
}