namespace TicTacToe.Game.Rules
{
    public interface IPlayer
    {
        string Name { get; }

        StepType StepType { get; }

        IStep MakeStep();
    }
}
