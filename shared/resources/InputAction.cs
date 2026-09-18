using Godot;

namespace Shared.Resources;

/// <summary>
/// This class stores string literals of all the input actions
/// a player can use. We want to use this class to maintain 
/// consistency.
/// </summary>
public static class InputAction
{
    public static readonly string LJSLeft = "ljs_left";
    public static readonly string LJSRight = "ljs_right";
    public static readonly string LJSUp = "ljs_up";
    public static readonly string LJSDown = "ljs_down";

    public static readonly string RJSLeft = "rjs_left";
    public static readonly string RJSRight = "rjs_right";
    public static readonly string RJSUp = "rjs_up";
    public static readonly string RJSDown = "rjs_down";

    public static readonly string ZoomIn = "zoom_in";
    public static readonly string ZoomOut = "zoom_out";
    
    public static readonly string Pause = "pause";

    public static readonly string OpenConsole = "open_console";

}    