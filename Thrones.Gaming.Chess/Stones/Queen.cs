using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class Queen : Stone
    {
        public Queen(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public override bool TryMove(Location target, Table table, out IStone willEated)
        {
            willEated = default;
            if (CheckMove(target) == false)
            {
                return false;
            }

            var targetLocationStone = table.Stones.GetFromLocation(target);
            var span = target - Location;

            // çapraz gidiş
            if (span.XDiff == span.YDiff)
            {
                var result = MovementRules.DiagonalCheck(Location, target, table);
                if (result == false)
                {
                    return result;
                }

                if (targetLocationStone != null)
                {
                    willEated = targetLocationStone;
                }
            }

            // yatay || dikey gidiş
            if (span.XDiff == 0 || span.YDiff == 0)
            {
                var result = MovementRules.HorizontalOrVerticalCheck(Location, target, table);
                if (result == false)
                {
                    return result;
                }

                if (targetLocationStone != null)
                {
                    willEated = targetLocationStone;
                }
            }

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

            if (span.XDiff == 0 && span.YDiff == 0)
            {
                return false;
            }

            if (span.XDiff != 0 && span.YDiff != 0 && span.XDiff != span.YDiff)
            {
                return false;
            }

            return true;
        }
    }
}
