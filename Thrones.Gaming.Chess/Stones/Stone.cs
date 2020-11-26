using System;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public abstract class Stone : IStone
    {
        public string Name { get; protected set; }
        public bool CouldMove { get; protected set; }
        public EnumStoneColor Color { get; protected set; }
        public Location Location { get; protected set; }
        public Player Player { get; protected set; }
        public int MoveCount { get; protected set; }
        public string NameWithColorPrefix => $"{GetType().Name.ToLower()}#{Color.ToString().ToLower()[0]}";

        public Stone(
            string name, 
            bool couldMove, 
            EnumStoneColor color, 
            Location location,
            Player player)
        {
            Name = name;
            CouldMove = couldMove;
            Color = color;
            Location = location;
            Player = player;
            MoveCount = 0;
        }

        protected abstract bool CheckMove(Location target);

        public abstract bool Move(Location target, Table table, out IStone eated);

        protected virtual void Move(Location location)
        {
            Location = location;
            MoveCount++;
        }
    }
}
