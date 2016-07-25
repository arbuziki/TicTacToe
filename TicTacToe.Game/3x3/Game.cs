using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Game.Rules;

namespace TicTacToe.Game._3x3
{
    public class Game : IGame
    {
        private Random _random;

        private Stack<IStep> _history = new Stack<IStep>();
        private Field _field;

        private List<IPlayer> _players = new List<IPlayer>();

        #region Properties

        public IField Field
        {
            get { return _field; }
        }

        public IPlayer[] Players
        {
            get { return _players.ToArray(); }
        }

        public ICollection<IStep> History
        {
            get { return _history.ToList(); }
        }

        public bool IsRunning { get; private set; }

        public int StepsCount
        {
            get { return _history.Count; }
        }

        #endregion


        #region Events

        public event PlayerGoingMakeStepDelegate OnPlayerGoingMakeStep;
        public event PlayerMakedStepDelegate OnPlayerMakedStep;
        public event PlayerStepErrorDelegate OnPlayerStepError;
        public event GameOverDelegate OnGameOver;

        #endregion


        public Game(IPlayer[] players)
        {
            if (players == null)
                throw new ArgumentNullException("players");

            if (players.Count() != 2)
                throw new Exception("Игра расчитана на 2-х игроков.");

            _field = new Field(3, 3);

            _random = new Random();

            _players = new List<IPlayer>(players);
            _players.ForEach(T => T.BeginGame(this));
        }

        public void StartGame()
        {
            IsRunning = true;

            if (_players.Count != 2)
                throw new Exception("Не назначено 2 игрока.");

            if (_players.First().StepType == _players.Last().StepType)
                throw new Exception("Игрокам должны быть назначены разные типы ходов.");

            new Thread(() =>
            {
                try
                {
                    GameBody();
                }
                catch (Exception e)
                {

                }

                IsRunning = false;

            })
            {IsBackground = true}.Start();
        }

        private void GameBody()
        {
            var currentPlayer = Players.First(T => T.StepType == StepType.X);

            while (IsRunning)
            {
                IStep step = null;

                RaisePlayerGoingMakeStepEvent(currentPlayer);

                while (true)
                {
                    try
                    {
                    step = currentPlayer.MakeStep();
                    }
                    catch (Exception e)
                    {
                        //Если алгоритм расчета ходов начинает тупить, то делаем случайный ход.
                        step = MakeRandomStep(currentPlayer);
                    }

                    var msg = GetCorrectString(step);

                    if (string.IsNullOrEmpty(msg))
                    {
                        break;
                    }

                    RaisePlayerStepErrorEvent(msg);
                }

                _history.Push(step);

                _field.DrawStep(step.X, step.Y, step.Player.StepType);

                RaisePlayerMakesStepEvent(step);

                var winner = GetWinner();

                if (winner != null)
                {
                    RaiseGameOverEvent(winner);
                    IsRunning = false;
                    break;
                }

                if (_history.Count == 9)
                {
                    RaiseGameOverEvent(null);
                    break;
                }
                    

                currentPlayer = Players.Single(T => T != currentPlayer);
            }
        }

        private string GetCorrectString(IStep step)
        {
            if (step == null)
                return "Ход не был сделан.";

            if (_history.Any(T => T.X == step.X && T.Y == step.Y))
                return "В эту клетку уже был сделан ход.";

            return string.Empty;
        }

        private IPlayer GetWinner()
        {
            if (_history.Count() < 5)
                return null;

            var stepsByPlayer = _history.GroupBy(T => T.Player);

            foreach (var playerSteps in stepsByPlayer)
            {
                var column = playerSteps.GroupBy(T => T.X).FirstOrDefault(T => T.Count() == 3);

                if (column != null)
                {
                    //Игрок выигрывает по столбцу
                    _field.DrawLine(column.Key, 0, column.Key, 2);
                    return playerSteps.Key;
                }

                var row = playerSteps.GroupBy(T => T.Y).FirstOrDefault(T => T.Count() == 3);

                if (row != null)
                {
                    //Игрок выигрывает по строке
                    _field.DrawLine(0, row.Key, 2, row.Key);
                    return playerSteps.Key;
                }

                //Ну и простой подсчет по диагоналям
                bool[,] arr = new bool[3, 3];

                foreach (var step in playerSteps)
                {
                    arr[step.X, step.Y] = true;
                }

                if (arr[1, 1])
                {
                    if (arr[0, 0] && arr[2, 2])
                    {
                        _field.DrawLine(0, 0, 2, 2);
                        return playerSteps.Key;
                    }

                    if (arr[2, 0] && arr[0, 2])
                    {
                        _field.DrawLine(2, 0, 0, 2);
                        return playerSteps.Key;
                    }
                }
            }

            return null;
        }

        private IStep MakeRandomStep(IPlayer player)
        {
            var emptyCells = _field.EmptyCells.ToArray();

            var randomCell =  emptyCells[_random.Next(0, emptyCells.Count())];

            return new Step(randomCell.X, randomCell.Y, player);
        }

        #region RaiseEventsRegion

        private void RaisePlayerGoingMakeStepEvent(IPlayer player)
        {
            var callback = Interlocked.CompareExchange(ref OnPlayerGoingMakeStep, null, null);

            if (callback != null)
            {
                callback(player);
            }
        }

        private void RaisePlayerMakesStepEvent(IStep step)
        {
            var callback = Interlocked.CompareExchange(ref OnPlayerMakedStep, null, null);

            if (callback != null)
            {
                callback(step);
            }
        }

        private void RaisePlayerStepErrorEvent(string msg)
        {
            var callback = Interlocked.CompareExchange(ref OnPlayerStepError, null, null);

            if (callback != null)
            {
                callback(msg);
            }
        }

        private void RaiseGameOverEvent(IPlayer player)
        {
            var callback = Interlocked.CompareExchange(ref OnGameOver, null, null);

            if (callback != null)
            {
                callback(player == null, player);
            }
        }

        #endregion
    }
}