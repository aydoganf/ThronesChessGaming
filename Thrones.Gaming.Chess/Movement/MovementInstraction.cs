using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.Movement
{
    public class MovementInstraction
    {
        public IStone Stone { get; private set; }
        public Location Location { get; private set; }
        public Player Player { get; private set; }
        public Table Table { get; private set; }

        private MovementInstraction(IStone stone, Location location, Table table)
        {
            Stone = stone;
            Location = location;
            Table = table;
            Player = stone.Player;
        }

        public static MovementInstraction CreateOne(IStone stone, Location location, Table table)
        {
            return new MovementInstraction(stone, location, table);
        }

        public MovementResult TryDo()
        {
            IStone eated = default;
            if (Stone.Move(Location, Table, out eated) != true)
            {
                return new MovementResult(false, Stone, null, Location, "Oraya olmaz!");
            }

            return new MovementResult(true, Stone, eated, Location, "OK");
        }
    }
}
