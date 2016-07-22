using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Game.Rules
{
    public struct Cell
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Cell(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public override string ToString()
        {
            return string.Format("{0}_{1}", X, Y);
        }
    }
}
