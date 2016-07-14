namespace TicTacToe.Game.Rules
{
    public interface IGameManager
    {
        IGame BeginGame(IPlayer[] players);
    }
}
