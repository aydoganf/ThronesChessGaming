using System;
using System.Collections.Generic;
using System.Linq;
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
            // hedef lokasyonda kendi taşı var
            if (Player.Stones.FirstOrDefault(s => s.Location == target) != null)
            {
                return false;
            }

            var span = target - Location;

            if (span.XDiff > 1 || span.YDiff > 1)
            {
                return false;
            }

            return true;
        }

        public bool CouldRun(Table table)
        {
            IStone _s = null;

            // left
            var left = table.GetLocation(Location.X - 1, Location.Y);
            if (left == null || TryMove(left, table, out _s) == false)
            {
                return false;
            }

            // left-top
            var leftTop = table.GetLocation(Location.X - 1, Location.Y + 1);
            if (leftTop == null || TryMove(leftTop, table, out _s) == false)
            {
                return false;
            }

            // top
            var top = table.GetLocation(Location.X, Location.Y + 1);
            if (top == null || TryMove(top, table, out _s) == false)
            {
                return false;
            }

            // right-top
            var rightTop = table.GetLocation(Location.X + 1, Location.Y + 1);
            if (rightTop == null || TryMove(rightTop, table, out _s) == false)
            {
                return false;
            }

            // right
            var right = table.GetLocation(Location.X + 1, Location.Y);
            if (right == null || TryMove(right, table, out _s) == false)
            {
                return false;
            }

            // right-bottom
            var rightBottom = table.GetLocation(Location.X + 1, Location.Y - 1);
            if (rightBottom == null || TryMove(rightBottom, table, out _s) == false)
            {
                return false;
            }

            // bottom
            var bottom = table.GetLocation(Location.X, Location.Y - 1);
            if (bottom == null || TryMove(bottom, table, out _s) == false)
            {
                return false;
            }

            // left-bottom
            var leftBottom = table.GetLocation(Location.X - 1, Location.Y - 1);
            if (leftBottom == null || TryMove(leftBottom, table, out _s) == false)
            {
                return false;
            }

            return true;
        }
    }
}
