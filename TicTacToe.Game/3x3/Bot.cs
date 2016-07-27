using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Game.Rules;

namespace TicTacToe.Game._3x3
{
    public class Bot : IPlayer
    {
        private IGame _game;

        private bool _swapAlgorithm;

        private Random _random = new Random();

        private StepType _stepType;

        public string Name { get; private set; }

        public StepType StepType
        {
            get { return _stepType; }
            set
            {
                if (IsPlaying)
                    throw new Exception("Нельзя менять тип хода во время игры.");

                _stepType = value;
            }
        }

        public bool IsPlaying { get; private set; }

        public IGame Game { get; set; }


        public Bot(string name)
        {
            Name = name;
        }

        public void BeginGame(IGame game)
        {
            _game = game;
            IsPlaying = true;
        }

        public void ClearGame()
        {
            _game = null;
            IsPlaying = false;
            _swapAlgorithm = false;
        }

        public IStep MakeStep()
        {
            if (_game == null)
                throw new Exception("Игра не назначена");
            
            if (!_game.IsRunning)
                throw new Exception("Игра не запущена.");

            if (_game.StepsCount >= 9)
                throw new Exception("Свободные клетки закончились");

            if (_game.StepsCount == 0 && StepType == StepType.O)
            {
                //Если так вышло, что мы ходим первыми, но почему-то ставим нолики, то меняем алгоритмы.
                _swapAlgorithm = true;
                Debugger.Break();
            }

            if ((_game.StepsCount % 2) == 0 && StepType != StepType.X)
            {
                throw new Exception("Крестики должны ходить первыми.");
            }

            if ((_game.StepsCount % 2) != 0 && StepType != StepType.O)
            {
                throw new Exception("Нолики должны ходить вторыми.");
            }

            var result = StepType == StepType.X && !_swapAlgorithm ? ProcessX() : ProcessO();

            if (result == null)
                throw new Exception("Ход не был сделан.");

            return result;
        }

        /// <summary>
        /// Алгоритм за крестики.
        /// </summary>
        /// <returns></returns>
        private IStep ProcessX()
        {
            IStep result = StepInWinCell();

            if (result != null)
                return result;

            //Сначала ходим в опасные ячейки (если они есть)
            result = StepInDangerCell();

            if (result != null)
                return result;
            //Затем - в центр
            result = StepInCenter();

            if (result != null)
                return result;

            if (_game.History.Count(T => T.Player != this) == 1)
            {
                var enemyStep = _game.History.Single(T => T.Player != this);

                if (enemyStep.X != 1 && enemyStep.Y != 1)
                    return new Step(2 - enemyStep.X, 2 - enemyStep.Y, this);

                int x = 1, y = 1;

                if (enemyStep.X == 1)
                {
                    x = _random.Next(0, 2) == 1 ? 2 : 0;
                    y = 2 - enemyStep.Y;
                }

                if (enemyStep.Y == 1)
                {
                    x = 2 - enemyStep.X;
                    y = _random.Next(0, 2) == 1 ? 2 : 0;
                }

                if (!_game.Field[x, y].HasValue)
                    return new Step(x, y, this);
            }

            result = StepInRandomCorner();

            if (result != null)
                return result;
            //Если все углы заняты - то в случайную клетку
            result = StepInRandomCell();

            return result;
        }

        private bool _easyAlgorithm;

        /// <summary>
        /// Алгоритм за нолики.
        /// </summary>
        /// <returns></returns>
        private IStep ProcessO()
        {
            IStep result = null;

            if (_game.StepsCount == 1)
            {
                var enemyStep = _game.History.Single();

                //Если первый ход был сделан в центр, то будет простой алгоритм (нолики ставим в углы, а если это не возможно - в любую клетку)
                _easyAlgorithm = enemyStep.X == 1 && enemyStep.Y == 1;

                if (_easyAlgorithm)
                {
                    return StepInRandomCorner();
                }
                else
                {
                    return StepInCenter();
                }
            }

            result = StepInWinCell();

            if (result != null)
                return result;

            result = StepInDangerCell();

            if (result != null)
                return result;

            if (_easyAlgorithm)
            {
                result = StepInRandomCorner();

                if (result != null)
                    return result;

                result = StepInRandomCell();
            }
            else
            {
                if (_game.History.Count(T => T.Player == this) == 1)
                {
                    var enemyStep = _game.History.FirstOrDefault(T => T.Player != this);

                    if (enemyStep.X == 1)
                    {
                        return new Step(_random.Next(0, 2) == 1 ? 2 : 0, enemyStep.Y, this);
                    }

                    if (enemyStep.Y == 1)
                    {
                        return new Step(enemyStep.X, _random.Next(0, 2) == 1 ? 2 : 0, this);
                    }

                    int x = _random.Next(0, 2) == 1 ? 2 : 0;
                    int y = _random.Next(0, 2) == 1 ? 2 : 0;

                    if (_random.Next(0, 2) == 1)
                        x = 1;
                    else
                    {
                        y = 1;
                    }

                    return new Step(x, y, this);
                }

                result = StepInRandomCell();
            }

            return result;
        }

        private IStep StepInWinCell()
        {
            var mySteps = _game.History.Where(T => T.Player.StepType == StepType).ToList();

            var column = mySteps.GroupBy(T => T.X).FirstOrDefault(T => T.Count() == 2);

            if (column != null)
            {
                int x = column.First().X;
                int y = 3 - column.Sum(T => T.Y);

                if (!_game.Field[x, y].HasValue)
                    return new Step(x, y, this);
            }

            var row = mySteps.GroupBy(T => T.Y).FirstOrDefault(T => T.Count() == 2);

            if (row != null)
            {
                int x = 3 - row.Sum(T => T.X);
                int y = row.First().Y;

                if (!_game.Field[x, y].HasValue)
                    return new Step(x, y, this);
            }

            //Проверяем по диагонали

            List<IStep> diag1 = new List<IStep>();
            List<IStep> diag2 = new List<IStep>();

            for (int i = 0; i < 2; i++)
            {
                var step = mySteps.FirstOrDefault(T => T.X == i && T.Y == i);

                if (step != null)
                    diag1.Add(step);

                step = mySteps.FirstOrDefault(T => T.X == 2 - i && T.Y == i);

                if (step != null)
                    diag2.Add(step);
            }

            if (diag1.Count == 2)
            {
                int x = 3 - diag1.Sum(T => T.X);
                int y = 3 - diag1.Sum(T => T.Y);

                if (!_game.Field[x, y].HasValue)
                    return new Step(x, y, this);
            }

            if (diag2.Count == 2)
            {
                int x = 3 - diag2.Sum(T => T.X);
                int y = 3 - diag2.Sum(T => T.Y);

                if (!_game.Field[x, y].HasValue)
                    return new Step(x, y, this);
            }

            return null;
        }

        private IStep StepInDangerCell()
        {
            var enemySteps = _game.History.Where(T => T.Player.StepType != StepType).ToList();

            var column = enemySteps.GroupBy(T => T.X).FirstOrDefault(T => T.Count() == 2);

            if (column != null)
            {
                int x = column.First().X;
                int y = 3 - column.Sum(T => T.Y);

                if (!_game.Field[x, y].HasValue)
                    return new Step(x, y, this);
            }

            var row = enemySteps.GroupBy(T => T.Y).FirstOrDefault(T => T.Count() == 2);

            if (row != null)
            {
                int x = 3 - row.Sum(T => T.X);
                int y = row.First().Y;

                if (!_game.Field[x, y].HasValue)
                    return new Step(x, y, this);
            }

            //Проверяем по диагонали

            List<IStep> diag1 = new List<IStep>();
            List<IStep> diag2 = new List<IStep>();

            for (int i = 0; i < 2; i++)
            {
                var step = enemySteps.FirstOrDefault(T => T.X == i && T.Y == i);

                if (step != null)
                    diag1.Add(step);

                step = enemySteps.FirstOrDefault(T => T.X == 2 - i && T.Y == i);

                if (step != null)
                    diag2.Add(step);
            }

            if (diag1.Count == 2)
            {
                int x = 3 - diag1.Sum(T => T.X);
                int y = 3 - diag1.Sum(T => T.Y);

                if (!_game.Field[x, y].HasValue)
                    return new Step(x, y, this);
            }

            if (diag2.Count == 2)
            {
                int x = 3 - diag2.Sum(T => T.X);
                int y = 3 - diag2.Sum(T => T.Y);

                if (!_game.Field[x, y].HasValue)
                    return new Step(x, y, this);
            }

            return null;
        }

        private IStep StepInCenter()
        {
            if (!_game.Field[1,1].HasValue)
                return new Step(1,1,this);

            return null;
        }

        private IStep StepInRandomCorner()
        {
            List<Cell> cells = new List<Cell>();

            for (int i = 0; i <= 2; i+=2)
            {
                for (int j = 0; j <= 2; j+=2)
                {
                    if (!_game.Field[i, j].HasValue)
                        cells.Add(new Cell(i, j));
                }
            }

            return !cells.Any() ? null : new Step(cells[_random.Next(0, cells.Count)], this);
        }

        private IStep StepInRandomCell()
        {
            return new Step(_game.Field.EmptyCells.ToList()[_random.Next(0, _game.Field.EmptyCells.Count())], this);
        }
    }
}
