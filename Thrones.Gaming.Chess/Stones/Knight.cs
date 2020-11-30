using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class Knight : Stone
    {
        public Knight(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public override bool TryMove(Location target, Table table, out IStone willEated)
        {
            willEated = default;
            if (CheckMove(target) == false)
            {
                return false;
            }

            var targetLocationStone = table.Stones.FirstOrDefault(s => s.Location == target);
            if (targetLocationStone != null)
            {
                willEated = targetLocationStone;
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

            if (target.X < 0 || target.Y < 0)
            {
                return false;
            }

            var span = target - Location;
            if ((span.XDiff == 1 && span.YDiff == 2) || (span.XDiff == 2 && span.YDiff == 1))
            {
                return true;
            }

            return false;
        }
    }
}
