using System;

namespace Shared.Managers.Dev;

/// <summary>
/// 
/// </summary>
public static class CheatConsoleBus
{
    private static Action openConsoleEvent;
    private static Action closeConsoleEvent;

    public static Action OpenConsoleEvent { get => openConsoleEvent; set => openConsoleEvent = value; }
    public static Action CloseConsoleEvent { get => closeConsoleEvent; set => closeConsoleEvent = value; }

    public static void BroadcastOpenConsoleEvent()
    {
        OpenConsoleEvent?.Invoke();
    }
    public static void BroadcastCloseConsoleEvent()
    {
        CloseConsoleEvent?.Invoke();
    }
}