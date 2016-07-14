using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Game.Rules;

namespace TicTacToe.Game._3x3
{
    public class Step : IStep
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public IPlayer Player { get; private set; }

        public Step(int x, int y, IPlayer player)
        {
            if (!IsIndexCorrect(X))
                throw new ArgumentOutOfRangeException("x");

            if (!IsIndexCorrect(y))
                throw new ArgumentOutOfRangeException("y");

            if (player == null)
                throw new ArgumentNullException("player");

            X = x;
            Y = y;
            Player = player;
        }

        private bool IsIndexCorrect(int index)
        {
            return index >= 0 && index <= 2;
        }

        public override string ToString()
        {
            return string.Format("({0},{1}) - {2}", X, Y, Player.StepType);
        }
    }
}
