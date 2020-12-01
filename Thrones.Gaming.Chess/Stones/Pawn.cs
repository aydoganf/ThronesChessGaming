using System.Collections.Generic;
using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class Pawn : Stone
    {
        internal Pawn(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public Pawn(EnumStoneColor color, int x, int y) : base(string.Empty, true, color, SessionFactory.GetTable().GetLocation(x, y), null)
        {
        }

        public override List<Location> GetMovementLocations(Location target, Table table)
        {
            List<Location> result = null;

            if (CheckMove(target, table))
            {
                result = new List<Location>();
                result.Add(target);
            }

            return result;
        }

        public override bool TryMove(Location target, Table table, out IStone willEated)
        {
            willEated = default;
            if (CheckMove(target, table) == false)
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

                willEated = enemyStone;
            }
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

            // piyon sadece ileri gidebilir.
            if (Color == EnumStoneColor.Black && span.YMovement == MovementDirection.Forward)
            {
                return false;
            }

            if (Color == EnumStoneColor.White && span.YMovement == MovementDirection.Backward)
            {
                return false;
            }

            // --------------------

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

            // çapraz gitme kuralı
            if (span.XMovement == MovementDirection.Backward || span.XMovement == MovementDirection.Forward)
            {
                var targetLocationStone = table.Stones.FirstOrDefault(s => s.Location == target);
                if (targetLocationStone == null || targetLocationStone.Player == this.Player)
                {
                    return false;
                }
            }

            // gitme yönünde 1 birim ötede bir taş varsa hareket edemez.
            if (span.XMovement == MovementDirection.None && table.Stones.FirstOrDefault(s => s.Location == target) != null)
            {
                return false;
            }

            return true;
        }
    }
}
