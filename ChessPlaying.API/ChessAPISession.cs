using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thrones.Gaming.Chess.SessionManagement;

namespace ChessPlaying.API
{
    public class ChessAPISession : Session
    {
        public override void DrawStatistics()
        {

        }

        public override void DrawTable()
        {
        }

        public override void ShowInfo()
        {
        }

        public override string WaitCommand()
        {
            return "";
        }

        public override void WriteEmpty()
        {
        }

        public override void WriteError(string error)
        {
        }

        public override void WriteLastCommand(string rawCommand)
        {
        }

        public override void WriteMessage(string message)
        {
        }
    }
}
