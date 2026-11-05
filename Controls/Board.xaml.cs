namespace Minesweeper.Controls
{
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml;
    using Microsoft.UI;
    using Microsoft.UI.Xaml.Media;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // To learn more about WinUI, the WinUI project structure,
    // and more about our project templates, see: http://aka.ms/winui-project-info.

    public sealed partial class Board : UserControl
    {
        private int totalCellCount;
        private double buttonSize = 24d;
        private BoardButton[,] grid;

        public int Rows
        {
            get;
            set;
        }

        public int Columns
        {
            get;
            set;
        }

        public int MineCount
        {
            get;
            set;
        }

        public Board()
        {
            InitializeComponent();
        }

        public void CreateEmptyBoard()
        {
            this.grid = new BoardButton[this.Rows, this.Columns];
            this.totalCellCount = this.Rows * this.Columns;
            int cellId = -1;

            for (int row = 0; row < this.Rows; row++)
            {
                for (int column = 0; column < this.Columns; column++)
                {
                    cellId++;
                    BoardButton button = new BoardButton();
                    this.grid[row, column] = button;
                    button.Row = row;
                    button.Column = column;
                    button.Padding = new Thickness(0);
                    button.Width = this.buttonSize;
                    button.Height = this.buttonSize;
                    button.Id = cellId;

                    Canvas.SetLeft(button, column * this.buttonSize);
                    Canvas.SetTop(button, row * this.buttonSize);
                    button.Tapped += this.InitializeMines;

                    this.boardCanvas.Children.Add(button);
                }
            }
        }

        private void InitializeMines(object sender, RoutedEventArgs e)
        {
            BoardButton seedButton = (BoardButton)sender;

            List<int> mineIds = new List<int>();
            Random random = new Random();

            List<int> cellIds = Enumerable.Range(1, this.totalCellCount + 1).ToList();
            List<BoardButton> seedNeighbors = seedButton.GetNeighbors(this.grid);
            seedNeighbors.Add(seedButton);

            foreach (BoardButton seed in seedNeighbors)
            {
                cellIds.Remove(seed.Id);
            }

            while (mineIds.Count < this.MineCount)
            {
                int mineId = random.Next(0, cellIds.Count);
                if (cellIds.Contains(mineId))
                {
                    cellIds.Remove(mineId);
                    mineIds.Add(mineId);
                }
            }

            for (int row = 0; row < this.Rows; row++)
            {
                for (int column = 0; column < this.Columns; column++)
                {
                    BoardButton button = this.grid[row, column];
                    button.Tapped -= this.InitializeMines;
                    button.Tapped += this.UnCoverCell;
                    button.RightTapped += this.FlagCell;

                    if (mineIds.Contains(button.Id))
                    {
                        button.IsMine = true;
                        button.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
            }

            for (int row = 0; row < this.Rows; row++)
            {
                for (int column = 0; column < this.Columns; column++)
                {
                    BoardButton button = this.grid[row, column];
                    if (button.IsMine == false)
                    {
                        List<BoardButton> neighbors = button.GetNeighbors(this.grid);
                        button.MinedNeighbors = neighbors.Count(n => n.IsMine);                        
                    }
                    else
                    {
                        button.MinedNeighbors = -1;
                    }
                }
            }

            this.UnCoverCell(seedButton, null);
        }

        private void UnCoverCell(object sender, RoutedEventArgs e)
        {
            BoardButton button = (BoardButton)sender;
            if (button.IsRevealed || button.IsEnabled == false)
            {
                return;
            }

            button.IsRevealed = true;

            button.IsEnabled = false;
            if (button.IsMine)
            {
                button.Content = "💣";
            }
            else
            {
                if (button.MinedNeighbors == 0)
                {
                    button.Content = "";
                    List<BoardButton> neighbors = button.GetNeighbors(this.grid);
                    foreach (BoardButton neighbor in neighbors)
                    {
                        this.UnCoverCell(neighbor, null);
                    }
                }
                else
                {
                    button.Content = button.MinedNeighbors.ToString();
                }
            }
        }

        private void FlagCell(object sender, RoutedEventArgs e)
        {
            BoardButton button = (BoardButton)sender;
            if (button.IsRevealed || button.IsEnabled == false)
            {
                return;
            }

            if (button.Content == null || button.Content.ToString() == "")
            {
                button.Content = "🚩";
            }
            else
            {
                button.Content = "";
            }
        }
    }
}