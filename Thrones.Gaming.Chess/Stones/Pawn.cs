using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class Pawn : Stone
    {
        public Pawn(
            string name, 
            bool couldMove, 
            EnumStoneColor color, 
            Location location,
            Player player) : base(name, couldMove, color, location, player)
        {
        }

        public override bool Move(Location target, Table table, out IStone eated)
        {
            eated = default;
            if (CheckMove(target) == false)
            {
                return false;
            }

            var span = target - Location;

            // çapraz hareket
            if (span.XDiff == 1 && span.YDiff == 1)
            {
                var enemyStone = table.Stones.FirstOrDefault(s => s.Location == target);
                if (enemyStone == null)
                {
                    return false;
                }

                eated = enemyStone;
            }

            base.Move(target);
            return true;
        }

        protected override bool CheckMove(Location target)
        {
            // hedef lokasyonda kendi taşı var
            if (Player.Stones.FirstOrDefault(s => s.Location == target) != null)
            {
                return false;
            }

            var span = target - Location;

            // piyon sadece ileri gidebilir.
            //if (span.Direction != MovementDirection.Forward)
            //{
            //    return false;
            //}

            // ilk hareketi değil ise bir kareden fazla gidemez
            if (MoveCount > 0 && span.XDiff > 1)
            {
                return false;
            }

            // hiçbir zaman 2 kareden fazla gidemez.
            if (span.YDiff > 2 || span.XDiff > 1)
            {
                return false;
            }

            // 2 birim X ekseninde yer değiştiriyorsa ilk hareket olması lazım
            if (span.YDiff == 2 && span.XDiff == 0 && MoveCount != 0)
            {
                return false;
            }

            return true;
        }
    }
}
