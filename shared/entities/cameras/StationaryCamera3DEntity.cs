using Godot;

namespace Shared.Entities.Cameras;

/// <summary>
/// This camera is a stationary camera that does not move or rotate.
/// It is meant to be used for cutscenes or other situations where 
/// the camera should not move.
/// </summary>
[GlobalClass]
public partial class StationaryCamera3DEntity : Camera3DEntity
{
}