using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Game.Rules;
using TicTacToe.Game._3x3;

namespace TicTacToe.Client.Cmd.Wrappers
{
    public class CmdGame
    {
        private IGameManager _gameManager;
        private IGame _game;

        private CmdField _cmdField;

        private List<IPlayer> _players = new List<IPlayer>(); 

        private readonly EventWaitHandle _threadLocker = new AutoResetEvent(false);

        public CmdGame(IGameManager gameManager)
        {
            if (gameManager == null)
                throw new ArgumentNullException("gameManager");

            _gameManager = gameManager;
        }

        public void ReqisterPlayer(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");

            if (_players.Contains(player))
                throw new Exception(string.Format("{0} уже был добавлен.", player));

            _players.Add(player);
        }

        public void Run()
        {
            if (_players.Count != 2)
                throw new Exception("Должно быть два игрока.");

            Random rnd = new Random();

            var player1 = _players[rnd.Next(0, 2)];
            var player2 = _players.Single(T => T != player1);

            player1.StepType = StepType.X;
            player2.StepType = StepType.O;

            Console.WriteLine("Игрок {0} играет за крестики.", player1.Name);
            Console.WriteLine("Игрок {0} играет за нолики.", player2.Name);

            _game = _gameManager.BeginGame(new[] { player1, player2 });

            _cmdField = new CmdField(_game.Field);

            _game.OnPlayerGoingMakeStep += OnPlayerGiongMakeStep;
            _game.OnPlayerStepError += OnPlayerStepError;
            _game.OnGameOver += OnGameOver;

            Console.WriteLine("Начинаем игру.");
            Console.WriteLine();

            _cmdField.RedrawField();
            Console.WriteLine();

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

        private void OnGameOver(bool isDraw, IPlayer winner)
        {
            if (isDraw)
            {
                Console.WriteLine("Игра закончена. Ничья!");
            }
            else
            {
                Console.WriteLine("Игра закончена. Выиграл: {0}.", winner.Name); 
            }

            _threadLocker.Set();
        }
    }
}
