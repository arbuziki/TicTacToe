using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Game
{
    /// <summary>
    /// Исключение автоматического расчета ходов.
    /// </summary>
    public class AutoProcessStepException : Exception
    {
        public AutoProcessStepException()
        {
        }

        public AutoProcessStepException(string message)
            : base(message)
        {
        }

        public AutoProcessStepException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
