using System;
using System.Linq;
using TicTacToe.Game.Rules;
using TicTacToe.Game._3x3;

namespace TicTacToe.Client.Cmd.Wrappers
{
    public class UserPlayer : IPlayer
    {
        private IGame _game;

        public string Name { get; private set; }
        public StepType StepType { get; private set; }

        public UserPlayer(string name, StepType stepType)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            Name = name;
            StepType = stepType;
        }

        public void BeginGame(IGame game)
        {
            _game = game;
        }

        public IStep MakeStep()
        {
            IStep result = null;

            do
            {
                Console.Write("Введите координаты ячейки (x, y): ");

                var inputString = Console.ReadLine();

                if (string.IsNullOrEmpty(inputString))
                    continue;

                var pointString = inputString.Replace("(", "").Replace(")", "").Split(',');

                if (pointString.Count() != 2)
                    continue;

                try
                {
                    int x = Convert.ToInt32(pointString.First().Trim());
                    int y = Convert.ToInt32(pointString.Last().Trim());

                    result = new Step(x, y, this);

                    return result;
                }
                catch
                { }
            }
            while (result == null);

            return result;
        }

        public override string ToString()
        {
            return string.Format("CMD-игрок \"{0}\" ({1})", Name, StepType);
        }
    }
}
