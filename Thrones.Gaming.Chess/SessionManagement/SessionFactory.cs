using System.Collections.Generic;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.Provider;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public enum SessionType
    {
        Console
    }

    public static class SessionFactory
    {
        private static ISession session;

        public static ISession CreateOne(string name, IGameProvider gameProvider, SessionType sessionType)
        {
            return sessionType switch
            {
                SessionType.Console => session = new ConsoleSession(name, gameProvider),
                _ => null,
            };
        }

        public static Table GetTable() => session.Table;
    }
}
