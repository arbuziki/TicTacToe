using System;
using TicTacToe.Game.Rules;

namespace TicTacToe.Client.Cmd.Wrappers
{
    public class CmdField
    {
        private const string TextField = "00|10|20\r\n01|11|21\r\n02|12|22";

        private readonly IField _field;

        public CmdField(IField field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            _field = field;

            field.OnFieldUpdated += OnFieldUpdated;
        }

        private void OnFieldUpdated()
        {
            Console.WriteLine();
            RedrawField();
            Console.WriteLine();
        }

        public void RedrawField()
        {
            string result = TextField;

            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    string symbol;

                    if (_field[j, i].HasValue)
                    {
                        symbol = _field[j, i].Value.ToString();
                    }
                    else
                    {
                        symbol = i == 2 ? " " : "_";
                    }

                    result = result.Replace(string.Format("{0}{1}", j, i), symbol);
                }
            }

            Console.WriteLine(result);
        }

    }
}
