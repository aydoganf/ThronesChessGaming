using System;
using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class Bishop : Stone
    {
        public Bishop(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public override bool Move(Location target, Table table, out IStone eated)
        {
            eated = default;
            if (CheckMove(target) == false)
            {
                return false;
            }

            int currentX = Location.X;
            int currentY = Location.Y;

            var span = target - Location;

            for (int i = 1; i <= span.XDiff; i++)
            {
                currentX += 1;
                currentY += 1;

                var checkLocation = table.Locations.FirstOrDefault(l => l.X == currentX && l.Y == currentY);
                if (checkLocation == null)
                {
                    return false;
                }

                var checkLocationStone = table.Stones.FirstOrDefault(s => s.Location == checkLocation);
                if (checkLocationStone != null)
                {
                    if (i != span.XDiff)
                    {
                        // son nokta değil arada taş var
                        return false;
                    }
                    else
                    {
                        eated = checkLocationStone;
                    }
                }
            }

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

            if (span.XDiff != span.YDiff)
            {
                return false;
            }

            return true;
        }
    }
}
