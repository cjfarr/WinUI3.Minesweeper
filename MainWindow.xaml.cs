namespace Minesweeper
{
    using Microsoft.UI.Xaml;

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.board.Rows = 16;
            this.board.Columns = 30;
            this.board.MineCount = 99;
            this.board.CreateEmptyBoard();
        }
    }
}
