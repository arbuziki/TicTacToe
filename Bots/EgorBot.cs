using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Game.Rules;

namespace Bots
{
    public class EgorBot : IPlayer
    {
        private IGame _game;
        private StepType _stepType;

        public string Name { get; private set; }

        public StepType StepType
        {
            get { return _stepType; }
            set
            {
                if (IsPlaying)
                    throw new Exception("Нельзя менять тип хода во время игры");

                _stepType = value;
            }
        }

        public bool IsPlaying { get; private set; }

        public void BeginGame(IGame game)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            _game = game;
            IsPlaying = true;
        }

        public IStep MakeStep()
        {
            //Здесь логика принятия решения.

            return null;
        }

        public void ClearGame()
        {
            _game = null;
            IsPlaying = false;
        }
    }
}
