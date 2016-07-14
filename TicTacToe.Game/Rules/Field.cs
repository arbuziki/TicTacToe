using System;

namespace TicTacToe.Game.Rules
{
    public class Field : IField
    {
        private readonly StepType?[,] _field;

        public int XCellsCount { get; private set; }
        public int YCellsCount { get; private set; }

        public StepType? this[int x, int y]
        {
            get { return _field[x, y]; }
        }

        public event FieldUpdatedDelegate OnFieldUpdated;

        public Field(int xCellsCount, int yCellsCount)
        {
            if (xCellsCount < 3)
                throw new ArgumentOutOfRangeException("xCellsCount");

            if (yCellsCount < 3)
                throw new ArgumentOutOfRangeException("yCellsCount");

            XCellsCount = xCellsCount;
            YCellsCount = yCellsCount;

            _field = new StepType?[xCellsCount, yCellsCount];
        }

        public void DrawStep(int x, int y, StepType stepType)
        {
            _field[x, y] = stepType;

            RaiseFieldUpdatedEvent();
        }

        public void DrawLine(int x0, int y0, int x1, int y1)
        {

            RaiseFieldUpdatedEvent();
        }

        private void RaiseFieldUpdatedEvent()
        {
            var callback = OnFieldUpdated;

            if (callback != null)
                callback();
        }
    }
}
