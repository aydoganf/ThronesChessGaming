using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class Rook : Stone
    {
        public Rook(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public override bool Move(Location target, Table table, out IStone eated)
        {
            eated = default;
            if (CheckMove(target) == false)
            {
                return false;
            }

            if (MovementRules.HorizontalOrVerticalCheck(Location, target, table) == false)
            {
                return false;
            }

            eated = table.Stones.FirstOrDefault(s => s.Location == target);
            //var span = target - Location;

            //int currentX = Location.X;
            //int currentY = Location.Y;
            //int targetLocationBorder = span.YDiff == 0 ? span.XDiff : span.YDiff;

            //for (int i = 1; i <= targetLocationBorder; i++)
            //{
            //    int checkX = currentX;
            //    int checkY = currentY;

            //    if (span.YDiff == 0)
            //    {
            //        checkX += 1;
            //    }

            //    if (span.XDiff == 0)
            //    {
            //        checkY += 1;
            //    }

            //    var checkLocation = table.Locations.FirstOrDefault(l => l.X == checkX && l.Y == checkY);
            //    if (checkLocation == null)
            //    {
            //        return false;
            //    }

            //    var nextLocationStone = table.Stones.FirstOrDefault(s => s.Location == checkLocation);
            //    if (nextLocationStone != null)
            //    {
            //        if (i != targetLocationBorder)
            //        {
            //            return false;
            //        }
            //        else
            //        {
            //            // son lokasyon ve düşman taşı var, ye oni
            //            eated = nextLocationStone;
            //        }
            //    }
            //}

            base.Move(target);
            return true;
        }

        protected override bool CheckMove(Location target)
        {
            // gidilecek location'da kendi taşı varsa
            if (Player.Stones.FirstOrDefault(s => s.Location == target) != null)
            {
                return false;
            }

            var span = target - Location;

            if (span.XDiff > 0 && span.YDiff > 0)
            {
                return false;
            }

            return true;
        }
    }
}
