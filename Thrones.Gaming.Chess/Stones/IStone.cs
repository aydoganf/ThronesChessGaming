using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public interface IStone
    {
        string NameWithColorPrefix { get; }
        string Name { get; }
        Location Location { get; }
        Player Player { get; }

        bool Move(Location target, Table table, out IStone eated);
    }
}
