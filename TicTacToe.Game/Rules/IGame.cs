using System.Collections.Generic;

namespace TicTacToe.Game.Rules
{
    public delegate void PlayerGoingMakeStepDelegate(IPlayer player);
    public delegate void PlayerStepErrorDelegate(string message);
    public delegate void PlayerMakedStepDelegate(IStep step);
    public delegate void GameOverDelegate(IPlayer winner);

    /// <summary>
    /// Описывает правила игры.
    /// </summary>
    public interface IGame
    {
        IField Field { get; }

        IPlayer[] Players { get; }
        /// <summary>
        /// История ходов игроков.
        /// </summary>
        ICollection<IStep> History { get; }

        int StepsCount { get; }

        bool IsRunning { get; }

        #region Events

        event PlayerGoingMakeStepDelegate OnPlayerGoingMakeStep;
        event PlayerMakedStepDelegate OnPlayerMakedStep;
        event PlayerStepErrorDelegate OnPlayerStepError;

        /// <summary>
        /// Игра завершена.
        /// </summary>
        event GameOverDelegate OnGameOver;

        #endregion

        void StartGame();
    }
}
