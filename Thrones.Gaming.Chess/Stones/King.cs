using System;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class King : Stone
    {
        public King(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public override bool Move(Location target, Table table, out IStone eated)
        {
            eated = default;
            throw new NotImplementedException();
        }

        protected override bool CheckMove(Location target)
        {
            var span = target - Location;

            if (span.XDiff > 1 || span.YDiff > 1)
            {
                return false;
            }

            if ((span.XDiff == 1 && span.YDiff != 0) || (span.YDiff == 1 && span.XDiff != 0))
            {
                return false;
            }

            return true;
        }
    }
}
