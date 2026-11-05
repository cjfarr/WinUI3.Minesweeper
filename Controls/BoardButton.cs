namespace Minesweeper.Controls
{
    using Microsoft.UI.Xaml.Controls;
    using System;
    using System.Collections.Generic;

    internal class BoardButton : Button
    {
        public bool IsRevealed { get; set; }
        public bool IsMine { get; set; }
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int MinedNeighbors { get; set; }

        public List<BoardButton> GetNeighbors(BoardButton[,] grid)
        {
            List<BoardButton> neighbors = new List<BoardButton>();
            int minRow = Math.Max(0, this.Row - 1);
            int maxRow = Math.Min(grid.GetLength(0) - 1, this.Row + 1);
            int minColumn = Math.Max(0, this.Column - 1);
            int maxColumn = Math.Min(grid.GetLength(1) - 1, this.Column + 1);

            for (int row = minRow; row <= maxRow; row++)
            {
                for (int column = minColumn; column <= maxColumn; column++)
                {
                    if (!(row == this.Row && column == this.Column))
                    {
                        neighbors.Add(grid[row, column]);
                    }
                }
            }

            return neighbors;
        }
    }
}