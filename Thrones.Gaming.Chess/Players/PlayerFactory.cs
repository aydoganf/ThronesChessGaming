using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrones.Gaming.Chess.SessionManagement;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.Players
{
    public class PlayerFactory
    {
        /// <summary>
        /// Creates a new <see cref="PlayerBuilder"/> contains a new <see cref="Player"/> with given nickname and color
        /// </summary>
        /// <param name="nickname">
        /// Nickname of player
        /// </param>
        /// <param name="color">
        /// Color of player
        /// </param>
        /// <returns>
        /// <see cref="PlayerBuilder"/>
        /// </returns>
        public static PlayerBuilder CreateOne(string nickname, EnumStoneColor color)
        {
            var player = Player.CreateOne(nickname, color);

            return new PlayerBuilder(player);
        }


    }

    public class PlayerBuilder
    {
        private readonly Player _player;

        private Dictionary<Type, int> StoneTypeCountMap = new Dictionary<Type, int>()
        {
            { typeof(King), 1 },
            { typeof(Queen), 1 },
            { typeof(Bishop), 2 },
            { typeof(Knight), 2 },
            { typeof(Rook), 2 },
            { typeof(Pawn), 8 },
        };

        internal PlayerBuilder(Player player)
        {
            _player = player;
        }

        /// <summary>
        /// Adds the given type of stone to current player.
        /// </summary>
        /// <typeparam name="TStone">
        /// Type of Stone. (Ex: Rook)
        /// </typeparam>
        /// <param name="x">
        /// The x-coordinate of position of stone
        /// </param>
        /// <param name="y">
        /// The y-coordinate of position of stone
        /// </param>
        /// <returns>
        /// <see cref="PlayerBuilder"/>
        /// </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public PlayerBuilder AddStone<TStone>(int x, int y) where TStone : Stone
        {
            var stoneCount = _player.Stones.Count(s => s.GetType() == typeof(TStone));
            if (stoneCount == StoneTypeCountMap[typeof(TStone)])
            {
                throw new InvalidOperationException($"Player has already {stoneCount} piece of {typeof(TStone).Name}.");
            }

            IStone stone = (IStone)Activator.CreateInstance(typeof(TStone), _player.Color, x, y);
            _player.AddStone(stone);
            return this;
        }


        /// <summary>
        /// Builds the player and adds it to given session
        /// </summary>
        /// <param name="toSession">
        /// Session which player will added to.
        /// </param>
        /// <returns>
        /// <see cref="Player"/>
        /// </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Player Build(ISession toSession)
        {
            if (_player.Stones.Count(s => s is King) == 0)
            {
                throw new InvalidOperationException("Player has no King stone!");
            }

            toSession.AddPlayer(_player);
            return _player;
        }
    }
}
