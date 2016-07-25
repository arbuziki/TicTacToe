namespace TicTacToe.Game.Rules
{
    public interface IPlayer
    {
        string Name { get; }
        StepType StepType { get; set; }

        bool IsPlaying { get; }

        void BeginGame(IGame game);
        IStep MakeStep();
        void ClearGame();
    }
}
