using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Game.Rules;

namespace TicTacToe.Client.Cmd.Wrappers
{
    public class CmdGame
    {
        private IGame _game;

        private CmdField _cmdField;

        private readonly EventWaitHandle _threadLocker = new AutoResetEvent(false);

        public CmdGame(IGameManager gameManager)
        {
            if (gameManager == null)
                throw new ArgumentNullException("gameManager");

            var player1 = new UserPlayer("Player 1", StepType.X);
            var player2 = new UserPlayer("Player 2", StepType.O);

            _game = gameManager.BeginGame(new IPlayer[] {player1, player2});

            _cmdField = new CmdField(_game.Field);

            _game.OnPlayerGoingMakeStep += OnPlayerGiongMakeStep;
            _game.OnPlayerStepError += OnPlayerStepError;
            _game.OnGameOver += OnGameOver;

            Console.WriteLine("Начинаем игру.");
            Console.WriteLine();

            _cmdField.RedrawField();
            Console.WriteLine();
        }

        public void Run()
        {
            _game.StartGame();

            _threadLocker.WaitOne();
        }

        private  void OnPlayerGiongMakeStep(IPlayer player)
        {
            Console.WriteLine("Сейчас ходит игрок: {0}", player.Name);
        }

        private void OnPlayerStepError(string message)
        {
            Console.WriteLine(message);
        }

        private void OnGameOver(IPlayer winner)
        {
            Console.WriteLine("Игра закончена. Выиграл: {0}.", winner.Name);

            _threadLocker.Set();
        }
    }
}
