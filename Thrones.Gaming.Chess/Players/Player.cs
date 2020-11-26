using System;
using System.Collections.Generic;
using System.Text;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.Players
{
    public class Player
    {
        public string Nickname { get; private set; }
        public long Duration { get; private set; }
        public List<IStone> Stones { get; private set; }
        public List<IStone> Eats { get; private set; }
        public EnumStoneColor Color { get; private set; }

        private Player()
        {
        }

        public static Player CreateOne(string nickname, EnumStoneColor color)
        {
            var player = new Player();
            player.Nickname = nickname;
            player.Stones = new List<IStone>();
            player.Eats = new List<IStone>();
            player.Color = color;
            return player;
        }

        internal void SetStones(List<IStone> stones)
        {
            if (Stones.Count != 0)
            {
                throw new InvalidOperationException($"Player {Nickname} has already stones.");
            }

            Stones = stones;
        }

        internal void Eat(IStone stone)
        {
            stone.Player.Stones.Remove(stone);
            Eats.Add(stone);
        }
    }
}
