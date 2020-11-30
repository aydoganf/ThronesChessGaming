namespace Thrones.Gaming.Chess.SessionManagement
{
    public interface ISession
    {
        void Start();

        ISession AddPlayers(string blackPlayerNickname, string whitePlayerNickname);
    }
}