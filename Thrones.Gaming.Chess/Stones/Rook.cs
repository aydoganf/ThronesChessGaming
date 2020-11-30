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

        public override bool TryMove(Location target, Table table, out IStone willEated)
        {
            willEated = default;
            if (CheckMove(target) == false)
            {
                return false;
            }

            if (MovementRules.HorizontalOrVerticalCheck(Location, target, table) == false)
            {
                return false;
            }

            willEated = table.Stones.GetFromLocation(target);
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
