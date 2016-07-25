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
        public string Name { get; private set; }
        public StepType StepType { get; set; }
        public bool IsPlaying { get; private set; }

        public void BeginGame(IGame game)
        {
            throw new NotImplementedException();
        }

        public IStep MakeStep()
        {
            throw new NotImplementedException();
        }

        public void ClearGame()
        {
            throw new NotImplementedException();
        }
    }
}
