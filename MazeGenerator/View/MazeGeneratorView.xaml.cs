using MazeGenerator.ViewModel;
using System.Windows;

namespace MazeGenerator.View
{
    /// <summary>
    /// The MazeGeneratorView class represents the View for the Maze Generator.
    /// </summary>
    public partial class MazeGeneratorView : Window
    {
        #region Fields
        #endregion

        #region Constructors

        public MazeGeneratorView()
        {
            InitializeComponent();

            // Create the View Model.
            MazeGeneratorViewModel viewModel = new MazeGeneratorViewModel();
            DataContext = viewModel;    // Set the data context for all data binding operations.
        }
        
        #endregion

        #region Events
        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion
    }
}
