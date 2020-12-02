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
            //var startingCommands = new List<string>();
            //startingCommands.Add("play h7 h5");
            //startingCommands.Add("play e2 e3");
            //startingCommands.Add("play h5 h4");
            //startingCommands.Add("play f1 c4");
            //startingCommands.Add("play h4 h3");
            //startingCommands.Add("play d1 f3");
            //startingCommands.Add("play h3 g2");
            //startingCommands.Add("play f3 f7");

            //SessionFactory
            //    .CreateOne("test session", null, SessionType.Console)
            //    .AddPlayers("faruk", "ali")
            //    .AddStartingCommands(startingCommands.ToArray())
            //    .Start();


            var session = SessionFactory.CreateOne("test session", null, SessionType.Console);

            List<IStone> blackStones = new List<IStone>();
            List<IStone> whiteStones = new List<IStone>();

            // --- scn #1
            //blackStones.Add(new Queen(EnumStoneColor.Black, 3, 4));
            //blackStones.Add(new King(EnumStoneColor.Black, 4, 8));
            //blackStones.Add(new Knight(EnumStoneColor.Black, 7, 3));
            //blackStones.Add(new Rook(EnumStoneColor.Black, 8, 8));
            //whiteStones.Add(new King(EnumStoneColor.White, 4, 1));
            //whiteStones.Add(new Rook(EnumStoneColor.White, 5, 2));
            //whiteStones.Add(new Bishop(EnumStoneColor.White, 3, 1));

            // --- scn #2
            //blackStones.Add(new Queen(EnumStoneColor.Black, 3, 6));
            //blackStones.Add(new King(EnumStoneColor.Black, 3, 8));
            //blackStones.Add(new Knight(EnumStoneColor.Black, 7, 3));
            //blackStones.Add(new Rook(EnumStoneColor.Black, 8, 1));
            //whiteStones.Add(new King(EnumStoneColor.White, 4, 1));
            //whiteStones.Add(new Rook(EnumStoneColor.White, 5, 1));
            //whiteStones.Add(new Bishop(EnumStoneColor.White, 6, 4));

            // --- scn #3
            blackStones.Add(new King(EnumStoneColor.Black, 2, 8));
            blackStones.Add(new Queen(EnumStoneColor.Black, 1, 4));
            blackStones.Add(new Rook(EnumStoneColor.Black, 3, 8));
            blackStones.Add(new Rook(EnumStoneColor.Black, 8, 8));
            blackStones.Add(new Bishop(EnumStoneColor.Black, 4, 4));
            blackStones.Add(new Knight(EnumStoneColor.Black, 5, 4));
            
            whiteStones.Add(new King(EnumStoneColor.White, 2, 1));
            whiteStones.Add(new Queen(EnumStoneColor.White, 5, 1));
            whiteStones.Add(new Bishop(EnumStoneColor.White, 7, 5));
            whiteStones.Add(new Bishop(EnumStoneColor.White, 6, 3));


            session
                .AddPlayer("faruk", EnumStoneColor.Black, blackStones)
                .AddPlayer("ali", EnumStoneColor.White, whiteStones)
                .Start();
        }
    }
}
