using MazeGenerator.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MazeGenerator.Model
{
    /// <summary>
    /// The Maze class represents a computer-generated maze.
    /// The maze is generated using the Recursive Backtracker algorithm.
    /// </summary>
    public class Maze : NotificationBase
    {
        #region Fields
        
        private ObservableCollection<MazeCell> _mazeCells;  // The maze.
        private Stack<MazeCell> _mazeGeneratorStack;        // A stack used to generate a new maze.
        private MazeState _mazeState = MazeState.Default;   // The current state of the maze.
        private Random _randomNumberGenerator;              // A random number generator.

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Maze()
        {
            try
            {
                _randomNumberGenerator = new Random();  // Initialise random number generator.
                ResetMaze();                            // Reset the maze.
            }
            catch (Exception ex)
            {
                throw new Exception("Maze(): " + ex.ToString());
            }
        }

        #endregion

        #region Events
        #endregion

        #region Properties

        /// <summary>
        /// Gets the width of the maze (in cells).
        /// </summary>
        public int MazeWidthCells
        {
            get
            {
                return Constants.MazeWidth;
            }
        }

        /// <summary>
        /// Gets the height of the maze (in cells).
        /// </summary>
        public int MazeHeightCells
        {
            get
            {
                return Constants.MazeHeight;
            }
        }

        /// <summary>
        /// Gets the collection of maze cells.
        /// </summary>
        public ObservableCollection<MazeCell> MazeCells
        {
            get
            {
                if (_mazeCells == null)
                {
                    _mazeCells = new ObservableCollection<MazeCell>();
                    while (_mazeCells.Count != Constants.MazeWidth * Constants.MazeHeight)
                    {
                        _mazeCells.Add(new MazeCell());
                    }
                }

                return _mazeCells;
            }
            private set
            {
                _mazeCells = value;
                RaisePropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets the maze state.
        /// </summary>
        public MazeState MazeState
        {
            get
            {
                return _mazeState;
            }
            private set
            {
                _mazeState = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CanGenerateMaze");
                RaisePropertyChanged("CanResetMaze");
            }
        }

        /// <summary>
        /// Gets a boolean flag indicating if a new maze can be generated.
        /// </summary>
        public bool CanGenerateMaze
        {
            get
            {
                return MazeState == MazeState.Default;
            }
        }
        
        /// <summary>
        /// Gets a boolean flag indicating if the simulation can be reset.
        /// </summary>
        public bool CanResetMaze
        {
            get
            {
                return MazeState == MazeState.MazeGenerated;
            }
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// The ResetMaze method is called to reset the maze.
        /// </summary>
        public void ResetMaze()
        {
            try
            {
                if (CanResetMaze)
                {
                    // Create and populate the maze.
                    MazeCells = new ObservableCollection<MazeCell>();
                    while (MazeCells.Count != Constants.MazeWidth * Constants.MazeHeight)
                    {
                        MazeCells.Add(new MazeCell());
                    }

                    MazeState = MazeState.Default;  // Reset the maze state.
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.ResetMaze(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The GenerateNewMaze method is called to generate a new maze.
        /// </summary>
        /// <returns></returns>
        public async Task GenerateNewMaze()
        {
            try
            {
                if (CanGenerateMaze)
                {
                    MazeState = MazeState.MazeGenerating;

                    // Clear the maze and stack.
                    ResetMaze();
                    _mazeGeneratorStack = new Stack<MazeCell>();

                    // Select a random cell to start.
                    MazeCell startCell = ChooseRandomCell();
                    startCell.CellState = CellState.Visited;

                    // Generate the new maze.
                    await GenerateNewMaze(startCell);
                    
                    // Set the start/end cells.
                    if (MazeCells.Count > 0)
                    {
                        MazeCells.First().CellType = CellType.Start;
                        MazeCells.Last().CellType = CellType.End;
                    }

                    MazeState = MazeState.MazeGenerated;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.GenerateNewMaze(): " + ex.ToString());
            }
        }

        /// <summary>
        /// The GenerateNewMaze method is called to generate a new maze.
        /// The Recursive Backtracker algorithm is used to generate the maze.
        /// http://weblog.jamisbuck.org/2010/12/27/maze-generation-recursive-backtracking
        /// https://en.wikipedia.org/wiki/Maze_generation_algorithm#Recursive_backtracker
        /// </summary>
        private async Task GenerateNewMaze(MazeCell currentCell)
        {
            try
            {
                await Task.Delay(Constants.MazeGenerationDelayMilliSeconds);

                if (MazeCells.Any(x => x.CellState == CellState.Default) || _mazeGeneratorStack.Count > 0)  // The maze contains unvisited cells or the stack is not empty.
                {
                    int cellIndex = MazeCells.IndexOf(currentCell);   // Retrieve the index of the current cell.

                    // Determine the indexes for the current cell's neighbours.
                    int northNeighbourIndex = cellIndex - Constants.MazeWidth;
                    int eastNeighbourIndex = cellIndex + 1;
                    int southNeighbourIndex = cellIndex + Constants.MazeHeight;
                    int westNeighbourIndex = cellIndex - 1;

                    // Determine if the current cell is on the north/east/south/west edge of the maze - certain neighbours must be ignored if the current cell is on an edge.
                    bool northEdge = cellIndex < Constants.MazeWidth ? true : false;
                    bool eastEdge = ((cellIndex + 1) % Constants.MazeWidth) == 0 ? true : false;
                    bool westEdge = (cellIndex % Constants.MazeWidth) == 0 ? true : false;
                    bool southEdge = (cellIndex + Constants.MazeWidth) >= (Constants.MazeWidth * Constants.MazeHeight) ? true : false;

                    // Retrieve the current cell's unvisited neighbours.
                    List<MazeCell> unvisitedNeighbours = new List<MazeCell>();
                    // North cell.
                    if (!northEdge && IsCellIndexValid(northNeighbourIndex) && MazeCells[northNeighbourIndex].CellState == CellState.Default)
                    {
                        unvisitedNeighbours.Add(MazeCells[northNeighbourIndex]);
                    }
                    // East cell.
                    if (!eastEdge && IsCellIndexValid(eastNeighbourIndex) && MazeCells[eastNeighbourIndex].CellState == CellState.Default)
                    {
                        unvisitedNeighbours.Add(MazeCells[eastNeighbourIndex]);
                    }
                    // South cell.
                    if (!southEdge && IsCellIndexValid(southNeighbourIndex) && MazeCells[southNeighbourIndex].CellState == CellState.Default)
                    {
                        unvisitedNeighbours.Add(MazeCells[southNeighbourIndex]);
                    }
                    // West cell.
                    if (!westEdge && IsCellIndexValid(westNeighbourIndex) && MazeCells[westNeighbourIndex].CellState == CellState.Default)
                    {
                        unvisitedNeighbours.Add(MazeCells[westNeighbourIndex]);
                    }

                    if (unvisitedNeighbours.Count > 0)
                    {
                        // The current cell has unvisited neighbours - select a random unvisited neighbour.
                        MazeCell selectedNeighbour = unvisitedNeighbours.ElementAt(_randomNumberGenerator.Next(unvisitedNeighbours.Count));

                        // Remove the wall between the current cell and the selected neighbour.
                        int selectedNeightbourIndex = MazeCells.IndexOf(selectedNeighbour);
                        if (selectedNeightbourIndex == northNeighbourIndex)
                        {
                            currentCell.RemoveWall(Direction.North);
                            selectedNeighbour.RemoveWall(Direction.South);
                        }
                        else if (selectedNeightbourIndex == eastNeighbourIndex)
                        {
                            currentCell.RemoveWall(Direction.East);
                            selectedNeighbour.RemoveWall(Direction.West);
                        }
                        else if (selectedNeightbourIndex == southNeighbourIndex)
                        {
                            currentCell.RemoveWall(Direction.South);
                            selectedNeighbour.RemoveWall(Direction.North);
                        }
                        else if (selectedNeightbourIndex == westNeighbourIndex)
                        {
                            currentCell.RemoveWall(Direction.West);
                            selectedNeighbour.RemoveWall(Direction.East);
                        }

                        // Put the current cell on the stack.
                        _mazeGeneratorStack.Push(currentCell);

                        // Set the selected neighbour as visited.
                        selectedNeighbour.CellState = CellState.Visited;

                        // Repeat the process with the selected neighbour as the new current cell.
                        await GenerateNewMaze(selectedNeighbour);
                    }
                    else
                    {
                        // Set the current cell to empty - it is now part of the maze.
                        currentCell.CellState = CellState.Empty;

                        // The current cell has no unvisited neighbours - pop a cell from the stack.
                        MazeCell previousCell = _mazeGeneratorStack.Pop();
                        previousCell.CellState = CellState.Empty; // Set the popped cell to empty - it is now part of the maze.

                        // Repeat the process with the popped cell as the new current cell.
                        await GenerateNewMaze(previousCell);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.GenerateNewMaze(MazeCell currentCell): " + ex.ToString());
            }
        }
        
        /// <summary>
        /// The ChooseRandomCell method is called to choose a random cell in the maze.
        /// </summary>
        /// <returns></returns>
        private MazeCell ChooseRandomCell()
        {
            try
            {
                // Select a random cell to start.
                int cellIndex = _randomNumberGenerator.Next(Constants.MazeHeight * Constants.MazeWidth);
                if (IsCellIndexValid(cellIndex))
                {
                    return MazeCells.ElementAt(cellIndex);
                }
                else
                {
                    throw new Exception("Unable to choose a randmom cell.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Maze.ChooseRandomCell(): " + ex.ToString());
            }
        }
        
        /// <summary>
        /// The IsCellIndexValid method is called to determine if a provided cell index is within range of the maze robot cell collection.
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        private bool IsCellIndexValid(int cellIndex)
        {
            return cellIndex >= 0 && cellIndex < MazeCells.Count;
        }
        
        #endregion
    }
}
