namespace TicTacToe.Game.Rules
{
    public interface IPlayer
    {
        string Name { get; }

        StepType StepType { get; }

        void BeginGame(IGame game);
        IStep MakeStep();
    }
}
