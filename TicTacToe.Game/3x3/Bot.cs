using System;
using System.Collections.Generic;
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

        private Random _random = new Random();

        public string Name { get; private set; }
        public StepType StepType { get; private set; }

        public IGame Game { get; set; }

        public Bot(string name, StepType stepType)
        {
            Name = name;
            StepType = stepType;
        }

        public void BeginGame(IGame game)
        {
            _game = game;
        }

        public IStep MakeStep()
        {
            if (_game == null)
                throw new Exception("Игра не назначена");
            
            if (!_game.IsRunning)
                throw new Exception("Игра не запущена.");

            if (_game.StepsCount >= 9)
                throw new Exception("Свободные клетки закончились");

            return StepType == StepType.X ? ProcessX() : ProcessO();
        }

        /// <summary>
        /// Алгоритм за крестики.
        /// </summary>
        /// <returns></returns>
        private IStep ProcessX()
        {
            //Сначала ходим в опасные ячейки (если они есть)
            IStep result = StepInDangerCell();

            if (result != null)
                return result;
            //Затем - в центр
            result = StepInCenter();

            if (result != null)
                return result;
            //Если он занят - в случайный угол
            result = StepInRandomCorner();

            if (result != null)
                return result;
            //Если все углы заняты - то в случайную клетку
            result = StepInRandomCell();

            return result;
        }

        /// <summary>
        /// Алгоритм за нолики.
        /// </summary>
        /// <returns></returns>
        private IStep ProcessO()
        {
            throw new NotImplementedException();
        }

        private IStep StepInDangerCell()
        {
            var enemySteps = _game.History.Where(T => T.Player.StepType != StepType);

            var column = enemySteps.GroupBy(T => T.X).FirstOrDefault(T => T.Count() == 2);

            if (column != null)
            {
                int y = 3 - column.Sum(T => T.Y);

                if (!_game.Field[column.First().X,y].HasValue)
                    return new Step(column.First().X, y, this);
            }

            var row = enemySteps.GroupBy(T => T.Y).FirstOrDefault(T => T.Count() == 2);

            if (row != null)
            {
                int x = 3 - row.Sum(T => T.X);
                if (!_game.Field[x, row.First().Y].HasValue)

                return new Step(x, row.First().Y, this);
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
