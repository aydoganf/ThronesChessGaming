﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.Provider;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public abstract class Session : ISession
    {
        public string Name { get; protected set; }
        public Table Table { get; protected set; }
        public List<Player> Players { get; protected set; }
        public Player CurrentPlayer { get; protected set; }
        public Player NextPlayer { get; protected set; }
        public Player PreviousPlayer => NextPlayer;
        private int CurrentIndexer = 0;

        private IGameProvider _gameProvider;
        public bool Check { get; protected set; }
        public IStone CheckStone { get; set; }

        private Stopwatch SessionTimer { get; set; }
        protected Dictionary<MovementInstraction, MovementResult> MovementInstractions { get; set; } = new Dictionary<MovementInstraction, MovementResult>();

        protected Session(string name, IGameProvider gameProvider) 
        {
            Name = name;
            Table = Table.CreateOne();
            Players = new List<Player>();
            _gameProvider = gameProvider;
        }

        public ISession AddPlayers(string blackPlayerNickname, string whitePlayerNickname)
        {
            string[] players = new string[2] { blackPlayerNickname, whitePlayerNickname };

            for (int i = 0; i < players.Length; i++)
            {
                var color = EnumStoneColor.Black;
                if (i == 1)
                {
                    color = EnumStoneColor.White;
                }
                string nickname = players[i];
                var player = Player.CreateOne(nickname, color, Table);

                #region Pawns

                int pawnLine = i == 0 ? 7 : 2;
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
            }
           
            return this;
        }


        public abstract void ShowInfo();
        public abstract void DrawTable();
        public abstract void WriteMessage(string message);
        public abstract void WriteError(string error);
        public abstract string WaitCommand();
        public abstract void DrawStatistics();

        public void Start()
        {
            CurrentPlayer = Players[CurrentIndexer];
            NextPlayer = Players[CurrentIndexer == 0 ? 1 : 0];
            DrawTable();
            string command = string.Empty;

            SessionTimer = new Stopwatch();
            SessionTimer.Start();

            while (command != "quit" && command != "exit")
            {
                if (string.IsNullOrEmpty(command))
                {
                    ShowInfo();
                }
                else
                {
                    if (command == "draw")
                    {
                        DrawTable();
                    }

                    if (command == "undo")
                    {
                        if (MovementInstractions.Keys.Count != 0)
                        {
                            var lastInstraction = MovementInstractions.Keys.Last();
                            var lastResult = MovementInstractions[lastInstraction];

                            if (lastResult.Eated != null)
                            {
                                CurrentPlayer.Stones.Add(lastResult.Eated);                                
                                PreviousPlayer.Eats.Remove(lastResult.Eated);
                            }

                            lastInstraction.MovingStone.ForceMove(lastInstraction.FromLocation);

                            SetPlayerReturn();

                            MovementInstractions.Remove(lastInstraction);
                            DrawTable();

                            SessionTimer.Restart();
                        }
                    }

                    if (command.StartsWith("play"))
                    {
                        var commandDetail = CommandResolver.Resolve(command);
                        if (commandDetail.IsCorrect == false)
                        {
                            WriteError(commandDetail.ReturnMessage);
                            command = WaitCommand();
                            continue;
                        }

                        var stone = CurrentPlayer.GetStone(commandDetail.From_X, commandDetail.From_Y);
                        var targetLocation = Table.GetLocation(commandDetail.To_X, commandDetail.To_Y);
                        
                        if (targetLocation == null)
                        {
                            WriteError("Location is not found!");
                            command = WaitCommand();
                            continue;
                        }

                        if (stone == null)
                        {
                            WriteError("Stone is not found at given location!");
                            command = WaitCommand();
                            continue;
                        }

                        var instraction = MovementInstraction.CreateOne(stone, targetLocation, this);
                        
                        var result = instraction.TryDo();
                        if (result.IsOK)
                        {
                            MovementInstractions.Add(instraction, result);

                            if (result.Eated != null)
                            {
                                CurrentPlayer.Eat(result.Eated);
                                Table.Stones.Remove(result.Eated);
                            }

                            if (result.CheckRemoved)
                            {
                                Check = false;
                            }

                            WriteMessage(result.Message);
                            IsCheck(stone);

                            SessionTimer.Stop();
                            CurrentPlayer.Duration += SessionTimer.ElapsedMilliseconds;

                            SetPlayerReturn();
                            DrawTable();

                            SessionTimer.Restart();
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

        protected void SetPlayerReturn()
        {
            int indexer = CurrentIndexer + 1;
            CurrentIndexer = indexer % 2;
            CurrentPlayer = Players[CurrentIndexer];
            NextPlayer = Players[CurrentIndexer == 0 ? 1 : 0];
        }

        private void IsCheck(IStone stone)
        {
            King king = NextPlayer.GetKing();
            if (stone.TryMove(king.Location, Table, out IStone k))
            {
                Check = true;
                CheckStone = stone;
                WriteMessage("CHECK");
            }
        }

    }
}
