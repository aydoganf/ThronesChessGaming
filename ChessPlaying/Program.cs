using System;
using Thrones.Gaming.Chess.SessionManagement;


namespace ChessPlaying
{
    class Program 
    {
        static void Main(string[] args)
        {
            SessionFactory
                .CreateOne("test session", null, SessionType.Console)
                .AddPlayers("faruk", "ali")
                .Start();
        }
    }
}
