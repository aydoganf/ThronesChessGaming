﻿using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.Movement
{
    public class MovementResult
    {
        public bool IsOK { get; private set; }
        public IStone Stone { get; private set; }
        public IStone Eated { get; private set; }
        public Location Location { get; private set; }
        public string Message { get; private set; }

        public MovementResult(bool isOk, IStone stone, IStone eated, Location location, string message)
        {
            IsOK = isOk;
            Stone = stone;
            Eated = isOk ? eated : null;
            Location = location;
            Message = message;
        }
    }
}