using System;

namespace Shared.Managers.Dev
{
    public static class DevConsoleManager
    {
        private static Action openDevConsoleEvent;
        private static Action devConsoleClosedEvent;

        public static Action OpenDevConsoleEvent { get => openDevConsoleEvent; set => openDevConsoleEvent = value; }
        public static Action DevConsoleClosedEvent { get => devConsoleClosedEvent; set => devConsoleClosedEvent = value; }

        public static void BroadcastOpenDevConsoleEvent()
        {
            OpenDevConsoleEvent?.Invoke();
        }
        public static void BroadcastDevConsoleClosedEvent()
        {
            DevConsoleClosedEvent?.Invoke();
        }
    }
}