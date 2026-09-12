using Godot;

namespace Shared.Resources;

/// <summary>
/// This class stores string literals of all the input actions
/// a player can use. We want to use this class to maintain 
/// consistency.
/// </summary>
public static class InputAction
{
    public static readonly string MoveLeft = "move_left";
    public static readonly string Right = "move_right";
    public static readonly string Up = "move_up";
    public static readonly string Down = "move_down";

    public static readonly string LookLeft = "look_left";
    public static readonly string LookRight = "look_right";
    public static readonly string LookUp = "look_up";
    public static readonly string LookDown = "look_down";
    public static readonly string LockOn = "lock_on";
    public static readonly string ZoomIn = "zoom_in";
    public static readonly string ZoomOut = "zoom_out";
    

    public static readonly string Dash = "dash";
    public static readonly string Attack = "attack";
    public static readonly string Special = "special";
    public static readonly string Parry = "parry";
    public static readonly string Jump = "jump";

    public static readonly string Respawn = "respawn";
    public static readonly string Interact = "interact";
    public static readonly string UICancel = "ui_cancel";
    public static readonly string Pause = "pause";

    public static readonly string DevConsole = "dev_console";
    public static readonly string DevNext = "dev_next";
    public static readonly string DevPrevious = "dev_previous";
    public static readonly string DevAutocomplete = "dev_autocomplete";
    public static readonly string DevEnter = "dev_enter";

}    