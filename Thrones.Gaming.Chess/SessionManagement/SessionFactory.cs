using Thrones.Gaming.Chess.Provider;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public enum SessionType
    {
        Console
    }

    public static class SessionFactory
    {
        public static ISession CreateOne(string name, IGameProvider gameProvider, SessionType sessionType)
        {
            return sessionType switch
            {
                SessionType.Console => new ConsoleSession(name, gameProvider),
                _ => null,
            };
        }
    }
}
