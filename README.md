# MazeGenerator
A random maze generator. Based on the Recursive Backtracker algorithm.

1. The maze begins as a grid of cells, with an empty stack.
2. Choose a random initial cell from the grid:
   1. Set this as the current cell and marked it as visited.
3. While there are unvisited cells in the grid:
   1. If the current cell has any unvisited neighbour cells:
      1. Choose a random unvisited neighbour cell.
      2. Remove the wall between the current cell and the chosen neighbour cell.
      3. Push the current cell onto the stack.
      4. Set the chosen neighbour cell as the current cell.
      5. Repeat step 3, with the new current cell.
   2. Else the current cell has no unvisited neighbour cells:
      1. Pop a cell from the stack, and set it as the current cell.
      2. Repeat step 3, with the new current cell.

A random maze is generated, with start and end locations.

For more information:
https://en.wikipedia.org/wiki/Maze_generation_algorithm#Recursive_backtracker
http://weblog.jamisbuck.org/2010/12/27/maze-generation-recursive-backtracking
