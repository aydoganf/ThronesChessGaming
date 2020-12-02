using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Provider;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.SessionManagement
{
    internal sealed class GhostSession : Session, IDisposable
    {
        public GhostSession(string name, IGameProvider gameProvider) : base(name, gameProvider)
        {
        }

        public override void DrawStatistics()
        {
            throw new NotImplementedException();
        }

        public override void DrawTable()
        {
            throw new NotImplementedException();
        }

        public override void ShowInfo()
        {
            throw new NotImplementedException();
        }

        public override string WaitCommand()
        {
            throw new NotImplementedException();
        }

        public override void WriteError(string error)
        {
            throw new NotImplementedException();
        }

        public override void WriteMessage(string message)
        {
            throw new NotImplementedException();
        }

        public static GhostSession CloneFrom(Session session)
        {
            return new GhostSession($"{session.Name}-ghost", null)
            {
                CurrentPlayer = session.CurrentPlayer,
                NextPlayer = session.NextPlayer,
                Players = session.Players,
                Table = session.Table,
                Check = session.Check,
                CheckStone = session.CheckStone
            };
        }

        public void Move(IStone stone, Location target)
        {
            ((IGhostStone)stone).Move(target);
            SetPlayerReturn();
        }



        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override void WriteEmpty()
        {
            throw new NotImplementedException();
        }
    }
}
