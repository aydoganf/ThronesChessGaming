using System;
using System.Collections.Generic;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class King : Stone
    {
        internal King(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public King(EnumStoneColor color, int x, int y) : base(string.Empty, true, color, SessionFactory.GetTable().GetLocation(x, y), null)
        {
        }

        public override List<Location> GetMovementLocations(Location target, Table table)
        {
            return null;
        }

        public override bool TryMove(Location target, Table table, out IStone willEated)
        {
            willEated = null;

            if (CheckMove(target, table) == false)
            {
                return false;
            }

            var span = target - Location;

            // yatay - dikey hareket
            if (span.XDiff == 0 || span.YDiff == 0)
            {
                if (MovementRules.HorizontalOrVerticalCheck(Location, target, table) == false)
                {
                    return false;
                }
            }

            // çapraz
            else
            {
                if (MovementRules.DiagonalCheck(Location, target, table) == false)
                {
                    return false;
                }
            }

            willEated = table.Stones.GetFromLocation(target);
            return true;
        }

        protected override bool CheckMove(Location target, Table table)
        {
            var span = target - Location;

            if (span.XDiff > 1 || span.YDiff > 1)
            {
                return false;
            }

            //if ((span.XDiff == 1 && span.YDiff != 0) || (span.YDiff == 1 && span.XDiff != 0))
            //{
            //    return false;
            //}

            return true;
        }
    }
}
