namespace TicTacToe.Game.Rules
{
    /// <summary>
    /// Описывает ход игрока.
    /// </summary>
    public interface IStep
    {
        /// <summary>
        /// Индекс ячейки по горизонтали.
        /// </summary>
        int X { get; }
        /// <summary>
        /// Индекс ячейки по вертикали.
        /// </summary>
        int Y { get; }
        /// <summary>
        /// Игрок, который сделал ход.
        /// </summary>
        IPlayer Player { get; }
    }
}
