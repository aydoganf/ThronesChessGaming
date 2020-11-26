using System;
using Thrones.Gaming.Chess.SessionManagement;


namespace ChessPlaying
{
    class Program
    {
        static void Main(string[] args)
        {

            var session = Session
                .CreateNewSession("test session")
                .AddPlayer("faruk")
                .AddPlayer("ali");


            session.Start();
        }
    }
}
