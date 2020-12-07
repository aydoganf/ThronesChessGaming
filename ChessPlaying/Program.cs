using System;using System.Collections.Generic;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Logging;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;
using Thrones.Gaming.Chess.Stones;

namespace ChessPlaying
{
    class Program 
    {
        static void Main(string[] args)
        {
            var startingCommands = new List<string>();
            //startingCommands.Add("play h7 h5");
            //startingCommands.Add("play e2 e3");
            //startingCommands.Add("play h5 h4");
            //startingCommands.Add("play f1 c4");
            //startingCommands.Add("play h4 h3");
            //startingCommands.Add("play d1 f3");
            //startingCommands.Add("play h3 g2");
            //startingCommands.Add("play f3 f7");

            //SessionFactory
            //    .CreateOne<ChessPlayingSession>("test-session")
            //    .AddPlayers("faruk", "ali")
            //    .AddStartingCommands(startingCommands.ToArray())
            //    .Start();

            string path = @"C:\Users\faruk\source\repos\ChessGame\ChessPlaying\session.json";
            string text = System.IO.File.ReadAllText(path);

            //string text = "{\"CurrentIndexer\": \"0\", \"FullTypeName\": \"ChessPlaying.ChessPlayingSession\", \"Name\":\"test-session\",\"Players\":[{\"Nickname\":\"faruk\"},{\"Nickname\":\"ali\"}],\"Table\":{\"Stones\":[{\"Type\":\"King\",\"Color\":\"Black\",\"Location\":\"4|8\"},{\"Type\":\"Queen\",\"Color\":\"Black\",\"Location\":\"3|8\"},{\"Type\":\"King\",\"Color\":\"White\",\"Location\":\"4|1\"},{\"Type\":\"Queen\",\"Color\":\"White\",\"Location\":\"3|1\"}]}}";

            var session = SessionFactory.CreateFrom(text);

            //PlayerFactory
            //    .CreateOne("faruk", EnumStoneColor.Black)
            //    .AddStone<King>(5, 8)
            //    .AddStone<Queen>(4, 8)
            //    .AddStone<Rook>(1, 8)
            //    .AddStone<Rook>(8, 8)
            //    .AddStone<Knight>(3, 5)
            //    .AddStone<Knight>(5, 5)
            //    .AddStone<Pawn>(1, 7)
            //    .AddStone<Pawn>(2, 7)
            //    .AddStone<Pawn>(5, 6)
            //    .AddStone<Pawn>(6, 6)
            //    .AddStone<Pawn>(8, 7)
            //    .AddStone<Pawn>(8, 5)
            //    .Build(session);

            //PlayerFactory
            //    .CreateOne("ali", EnumStoneColor.White)
            //    .AddStone<King>(5, 1)
            //    .AddStone<Queen>(2, 1)
            //    .AddStone<Bishop>(4, 2)
            //    .AddStone<Bishop>(6, 1)
            //    .AddStone<Pawn>(1, 3)
            //    .AddStone<Pawn>(3, 4)
            //    .AddStone<Pawn>(5, 4)
            //    .AddStone<Pawn>(6, 4)
            //    .Build(session);

            session.SetLogger(new FileLogger(Environment.CurrentDirectory));
            session.Start();

            string command = string.Empty;
            while (command != "quit" && command != "exit")
            {
                command = ((Session)session).WaitCommand();

                text = Newtonsoft.Json.JsonConvert.SerializeObject(session.Command(command));

                session = SessionFactory.CreateFrom(text);
            }


            //session.Start();

        }
    }
}
