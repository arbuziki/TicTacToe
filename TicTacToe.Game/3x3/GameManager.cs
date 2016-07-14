using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Game.Rules;

namespace TicTacToe.Game._3x3
{
    public class GameManager : IGameManager
    {
        public IGame BeginGame(IPlayer[] players)
        {
            return new Game(players);
        }
    }
}
