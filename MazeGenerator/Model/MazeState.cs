using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator.Model
{
    /// <summary>
    /// The MazeState enumeration represents the current state of the maze.
    /// </summary>
    public enum MazeState
    {
        Default,
        MazeGenerating,
        MazeGenerated
    }
}
