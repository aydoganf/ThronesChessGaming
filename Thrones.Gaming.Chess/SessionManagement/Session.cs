using System;
using System.Collections.Generic;
using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public class Session
    {
        public string Name { get; private set; }
        public Table Table { get; private set; }
        public List<Player> Players { get; private set; }

        public Player CurrentPlayer { get; private set; }
        private int CurrentIndexer = 0; 

        private Session()
        {
        }

        public static Session CreateNewSession(string name)
        {
            var session = new Session();
            session.Name = name;
            session.Table = Table.CreateOne();
            session.Players = new List<Player>();
            return session;
        }


        public Session AddPlayer(string nickname)
        {
            if (Players.Count == 2)
            {
                throw new InvalidOperationException("Session has already two players.");
            }

            var color = EnumStoneColor.Black;
            var player = Player.CreateOne(nickname, color);

            #region Pawns

            int pawnLine = 7;

            if (Players.Count == 1)
            {
                color = EnumStoneColor.White;
                pawnLine = 2;
            }

            List<IStone> stones = new List<IStone>();
            foreach (var xAxis in Table.xAxis.Keys)
            {
                var xAxisVal = Table.xAxis[xAxis];
                var name = $"pawn#{xAxisVal}{pawnLine}";
                var pawn = new Pawn(name, true, color, Table.GetLocation(xAxis, pawnLine), player);

                stones.Add(pawn);
            }
            #endregion

            #region Rooks

            Rook rookLeft = default;
            Rook rookRight = default;

            if (color == EnumStoneColor.Black)
            {
                rookLeft = new Rook("rook#a8", false, color, Table.GetLocation(1, 8), player);
                rookRight = new Rook("rook#h8", false, color, Table.GetLocation(8, 8), player);
            }
            else
            {
                rookLeft = new Rook("rook#a1", false, color, Table.GetLocation(1, 1), player);
                rookRight = new Rook("rook#h1", false, color, Table.GetLocation(8, 1), player);
            }

            stones.Add(rookLeft);
            stones.Add(rookRight);

            #endregion

            #region Knights
            Knight knightLeft = default;
            Knight knightRight = default;

            if (color == EnumStoneColor.Black)
            {
                knightLeft = new Knight("knight#b8", true, color, Table.GetLocation(2, 8), player);
                knightRight = new Knight("knight#g8", true, color, Table.GetLocation(7, 8), player);
            }
            else
            {
                knightLeft = new Knight("knight#b1", true, color, Table.GetLocation(2, 1), player);
                knightRight = new Knight("knight#g1", true, color, Table.GetLocation(7, 1), player);
            }

            stones.Add(knightLeft);
            stones.Add(knightRight);
            #endregion

            #region Bishops
            Bishop bishopLeft = default;
            Bishop bishopRight = default;

            if (color == EnumStoneColor.Black)
            {
                bishopLeft = new Bishop("bishop#c8", false, color, Table.GetLocation(3, 8), player);
                bishopRight = new Bishop("bishop#f8", false, color, Table.GetLocation(6, 8), player);
            }
            else
            {
                bishopLeft = new Bishop("bishop#c1", false, color, Table.GetLocation(3, 1), player);
                bishopRight = new Bishop("bishop#f1", false, color, Table.GetLocation(6, 1), player);
            }

            stones.Add(bishopLeft);
            stones.Add(bishopRight);
            #endregion

            #region Queens
            Queen queen = default;
            if (color == EnumStoneColor.Black)
            {
                queen = new Queen("queen", false, color, Table.GetLocation(4, 8), player);
            }
            else
            {
                queen = new Queen("queen", false, color, Table.GetLocation(4, 1), player);
            }

            stones.Add(queen);
            #endregion

            #region Kings
            King king = default;

            if (color == EnumStoneColor.Black)
            {
                king = new King("king", false, color, Table.GetLocation(5, 8), player);
            }
            else
            {
                king = new King("king", false, color, Table.GetLocation(5, 1), player);
            }

            stones.Add(king);
            #endregion

            
            player.SetStones(stones);

            Players.Add(player);
            Table.AddStones(stones);
            return this;
        }


        public void Start()
        {
            CurrentPlayer = Players[CurrentIndexer];
            DrawToConsole();
            string command = string.Empty;

            while (command != "quit")
            {
                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine();
                    Console.WriteLine("You can use play, draw and quit commands.");
                    Console.WriteLine("command: > play [from] [to] (ex: play a3 a5)");
                }
                else
                {
                    if (command == "draw")
                    {
                        Console.Clear();
                        DrawToConsole();
                    }

                    if (command.StartsWith("play"))
                    {
                        string[] commandArray = command.Split(" ");
                        string from = commandArray[1];
                        string to = commandArray[2];

                        if (from.Length != 2)
                        {
                            WriteError("from location parameter is wrong!");
                            command = WaitCommand();
                            continue;
                        }

                        if (to.Length != 2)
                        {
                            WriteError("from location parameter is wrong!");
                            command = WaitCommand();
                            continue;
                        }

                        if (Table.xAxisConverter.ContainsKey(from[0].ToString()) == false || Table.xAxisConverter.ContainsKey(to[0].ToString()) == false)
                        {
                            WriteError("Lokasyon yanlış!");
                            command = WaitCommand();
                            continue;
                        }

                        int fromX = Table.xAxisConverter[from[0].ToString()];
                        int fromY = int.Parse(from[1].ToString());

                        int toX = Table.xAxisConverter[to[0].ToString()];
                        int toY = int.Parse(to[1].ToString());


                        var stone = CurrentPlayer.Stones.FirstOrDefault(s => s.Location == Table.GetLocation(fromX, fromY));
                        var targetLocation = Table.GetLocation(toX, toY);

                        if (stone == null)
                        {
                            WriteError("Taş bulunamadı!");
                            command = WaitCommand();
                            continue;
                        }

                        if (targetLocation == null)
                        {
                            WriteError("Lokasyon bulunamadı!");
                            command = WaitCommand();
                            continue;
                        }

                        var instraction = Movement.MovementInstraction.CreateOne(stone, targetLocation, Table);
                        
                        var result = instraction.TryDo();
                        //stone.Move(targetLocation, Table);
                        if (result.IsOK)
                        {
                            if (result.Eated != null)
                            {
                                CurrentPlayer.Eat(result.Eated);
                                Table.Stones.Remove(result.Eated);
                                DrawToConsole();
                            }

                            int indexer = CurrentIndexer + 1;
                            CurrentIndexer = indexer % 2;
                            CurrentPlayer = Players[CurrentIndexer];
                            WriteMessage(result.Message);

                            Console.Clear();
                            DrawToConsole();
                        }
                        else
                        {
                            WriteError(result.Message);
                        }
                    }

                    if (command == "stat")
                    {
                        DrawStatistics();
                    }
                }

                command = WaitCommand();
            }
        }

        private void WriteMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} - [system]> {message}");
            Console.ResetColor();
        }

        private void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} - [system]> {error}");
            Console.ResetColor();
        }

        private string WaitCommand()
        {
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.Write($"{DateTime.Now} - [player: {CurrentPlayer.Nickname}]>");
            Console.ResetColor();
            return Console.ReadLine();
        }

        private void DrawToConsole()
        {
            List<IStone> allStones = new List<IStone>();
            allStones.AddRange(Players.SelectMany(i => i.Stones));

            Console.WindowHeight = 45;
            if (CurrentPlayer == Players[0])
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"                              {Players[0].Nickname} # duration: {Players[0].Duration}                              ");
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine("       a          b          c          d          e          f          g          h      ");
            Console.WriteLine(" _________________________________________________________________________________________ ");
            for (int yAxis = 8; yAxis > 0; yAxis--)
            {
                Console.Write(" |          ");
                for (int xAxis = 1; xAxis < 8; xAxis++)
                {
                    string show = "          ";
                    Console.Write($"|{show}");
                }
                Console.Write("|");
                Console.WriteLine();

                Console.Write($"{Table.yAxis[yAxis]}|");
                for (int xAxis = 1; xAxis <= 8; xAxis++)
                {
                    var location = Table.GetLocation(xAxis, yAxis);
                    var stone = allStones.FirstOrDefault(s => s.Location == location);

                    string show = "          ";
                    if (stone != null)
                    {
                        var name = stone.NameWithColorPrefix;
                        var kalan = 10 - name.Length;
                        string empty = "";
                        for (int i = 0; i < kalan; i++)
                        {
                            empty += " ";
                        }
                        show = name + empty;
                    }
                    Console.Write($"{show}|");
                }
                Console.Write($"{Table.yAxis[yAxis]}");
                Console.WriteLine();

                Console.Write(" |__________");
                for (int xAxis = 1; xAxis < 8; xAxis++)
                {
                    string show = "__________";
                    Console.Write($"|{show}");
                }
                Console.Write("|");
                Console.WriteLine();
            }
            Console.WriteLine("       a          b          c          d          e          f          g          h      ");

            Console.WriteLine();
            if (CurrentPlayer == Players[1])
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"                              {Players[1].Nickname} # duration: {Players[1].Duration}                              ");
            Console.ResetColor();
        }

        private void DrawStatistics()
        {
            foreach (var player in Players)
            {
                Console.WriteLine();

                Console.WriteLine($"   {player.Nickname}   ");
                Console.WriteLine("______________");

                foreach (var eat in player.Eats)
                {
                    Console.WriteLine($"> {eat.Name}");
                }

                Console.WriteLine();
            }
        }
    }
}
