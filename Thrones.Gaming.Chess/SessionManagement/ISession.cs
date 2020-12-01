using System.Collections.Generic;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public interface ISession
    {
        Table Table { get; }

        void Start();

        ISession AddPlayers(string blackPlayerNickname, string whitePlayerNickname);

        ISession AddPlayer(string nickname, EnumStoneColor color, List<IStone> stones);
    }
}