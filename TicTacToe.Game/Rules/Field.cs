using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace TicTacToe.Game.Rules
{
    public class Field : IField
    {
        private readonly StepType?[,] _field;
        private readonly List<Cell> _emptyCells; 

        public int XCellsCount { get; private set; }
        public int YCellsCount { get; private set; }

        public StepType? this[int x, int y]
        {
            get { return _field[x, y]; }
        }

        public IEnumerable<Cell> EmptyCells
        {
            get { return _emptyCells; }
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
            _emptyCells = GetFieldCells().ToList();
        }

        public void DrawStep(int x, int y, StepType stepType)
        {
            _field[x, y] = stepType;

            if (_emptyCells.Any(T => T.X == x && T.Y == y))
            {
                _emptyCells.Remove(new Cell (x, y));
            }

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

        private IEnumerable<Cell> GetFieldCells()
        {
            for (int i = 0; i < XCellsCount; i++)
            {
                for (int j = 0; j < YCellsCount; j++)
                {
                    yield return new Cell (i, j);
                }
            }
        }
    }
}
