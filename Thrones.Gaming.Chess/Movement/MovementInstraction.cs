using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.Movement
{
    public class MovementInstraction
    {
        public IStone MovingStone { get; private set; }
        public Location FromLocation { get; private set; }
        public Location Location { get; private set; }
        public Session Session { get; private set; }

        private MovementInstraction(IStone stone, Location location, Session session)
        {
            MovingStone = stone;
            Location = location;
            Session = session;
            FromLocation = stone.Location;
        }

        public static MovementInstraction CreateOne(IStone stone, Location location, Session session)
        {
            return new MovementInstraction(stone, location, session);
        }

        public MovementResult TryDo()
        {
            IStone willEated = default;
            if (MovingStone.TryMove(Location, Session.Table, out willEated) != true)
            {
                return new MovementResult(false, MovingStone, null, Location, "Oraya olmaz!");
            }

            bool isOk = true;
            bool checkRemoved = false;
            string message = string.Empty;

            if (Session.Check)
            {
                // oynanan taş king ise
                if (MovingStone is King)
                {
                    // check another stone could eat king
                }

                // başka bir taş oynanıyor ve yenilecek olan taş şah çekilen
                else if (willEated == Session.CheckStone)
                {
                    isOk = true;
                    checkRemoved = true;
                }

                // kralın önüne geçtiyse ve check-i bitirdiyse
                else
                {
                    // CurrentPlayer'ın MovingStone'u o lokasyona ghostMove yapar.
                    MovingStone.GhostMove(Location);

                    // Şuanki durumda CurrentPlayer'a sıra geçecektir.
                    var king = Session.CurrentPlayer.GetKing();
                    if (Session.CheckStone.TryMove(king.Location, Session.Table, out IStone _k))
                    {
                        // hala check-i kaldıramıyor.
                        isOk = false;
                    }
                    else
                    {
                        isOk = true;
                        checkRemoved = true;
                    }
                }
            }
            else
            {
                var kingLocation = Session.CurrentPlayer.GetKing().Location;
                MovingStone.GhostMove(Location);

                foreach (var enemyStone in Session.NextPlayer.Stones)
                {
                    var moveCheck = enemyStone.TryMove(kingLocation, Session.Table, out IStone _k);
                    if (moveCheck)
                    {
                        isOk = false;
                        message = "Protect your KING!";
                        break;
                    }
                }

                MovingStone.UndoGhost();
            }

            if (isOk)
            {
                MovingStone.Move(Location, Session.Table, out willEated);
                return new MovementResult(true, MovingStone, willEated, Location, "OK", checkRemoved);
            }

            return new MovementResult(false, MovingStone, null, Location, message);
        }
    }
}
