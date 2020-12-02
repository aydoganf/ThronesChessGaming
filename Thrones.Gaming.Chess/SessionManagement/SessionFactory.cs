using System;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public static class SessionFactory
    {
        private static ISession session;

        public static ISession CreateOne<TSession>(string name) where TSession : Session
        {
            var newSession = (Session)Activator.CreateInstance(typeof(TSession));

            newSession.SetName(name);
            session = newSession;
            return session;
        }

        public static Table GetTable() => session.Table;
    }
}
