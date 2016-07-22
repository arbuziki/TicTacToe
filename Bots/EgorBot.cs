using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Game.Rules;

namespace Bots
{
    public class EgorBot : IPlayer
    {
        public string Name { get; }
        public StepType StepType { get; }
        public void BeginGame(IGame game)
        {
            throw new NotImplementedException();
        }

        public IStep MakeStep()
        {
            throw new NotImplementedException();
        }
    }
}
