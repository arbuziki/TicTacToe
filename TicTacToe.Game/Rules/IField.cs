using System.Collections.Generic;
using System.ComponentModel;
using TicTacToe.Game._3x3;

namespace TicTacToe.Game.Rules
{
    public delegate void FieldUpdatedDelegate();

    /// <summary>
    /// Описывает игровое поле.
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Размерность поля по горизонтали.
        /// </summary>
        int XCellsCount { get; }
        /// <summary>
        /// Размерность поля по вертикали.
        /// </summary>
        int YCellsCount { get; }

        StepType? this[int x, int y] { get; }
        IEnumerable<Cell> EmptyCells { get; } 

        #region Events

        event FieldUpdatedDelegate OnFieldUpdated;

        #endregion

        void DrawStep(int x, int y, StepType stepType);
        void DrawLine(int x0, int y0, int x1, int y1);
    }
}
