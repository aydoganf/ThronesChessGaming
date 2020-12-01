using System;
using System.Collections.Generic;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.SessionManagement;
using Thrones.Gaming.Chess.Stones;

namespace ChessPlaying
{
    class Program 
    {
        static void Main(string[] args)
        {
            //SessionFactory
            //    .CreateOne("test session", null, SessionType.Console)
            //    .AddPlayers("faruk", "ali")
            //    .Start();

            var session = SessionFactory.CreateOne("test session", null, SessionType.Console);

            List<IStone> farukStones = new List<IStone>();

            farukStones.Add(new Queen(EnumStoneColor.Black, 3, 4));
            farukStones.Add(new King(EnumStoneColor.Black, 4, 8));
            farukStones.Add(new Knight(EnumStoneColor.Black, 5, 5));
            farukStones.Add(new Rook(EnumStoneColor.Black, 8, 8));


            List<IStone> aliStones = new List<IStone>();
            aliStones.Add(new King(EnumStoneColor.White, 5, 1));
            aliStones.Add(new Rook(EnumStoneColor.White, 1, 1));
            aliStones.Add(new Bishop(EnumStoneColor.White, 3, 1));


            session
                .AddPlayer("faruk", EnumStoneColor.Black, farukStones)
                .AddPlayer("ali", EnumStoneColor.White, aliStones)
                .Start();
        }
    }
}
