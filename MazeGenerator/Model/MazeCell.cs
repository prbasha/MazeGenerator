using MazeGenerator.Common;
using System;

namespace MazeGenerator.Model
{
    /// <summary>
    /// The MazeCell class represents a single maze cell.
    /// </summary>
    public class MazeCell : NotificationBase
    {
        #region Fields

        private bool _northWall = true;
        private bool _eastWall = true;
        private bool _southWall = true;
        private bool _leftWall = true;
        private CellState _cellState = CellState.Default;
        private CellType _cellType = CellType.Normal;
        private bool _containsRobot = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MazeCell()
        {
        }
        
        #endregion

        #region Events
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the north wall of the cell.
        /// </summary>
        public bool NorthWall
        {
            get
            {
                return _northWall;
            }
            private set
            {
                _northWall = value;
                RaisePropertyChanged("NorthWall");
            }
        }

        /// <summary>
        /// Gets or sets the east wall of the cell.
        /// </summary>
        public bool EastWall
        {
            get
            {
                return _eastWall;
            }
            private set
            {
                _eastWall = value;
                RaisePropertyChanged("EastWall");
            }
        }

        /// <summary>
        /// Gets or sets the south wall of the cell.
        /// </summary>
        public bool SouthWall
        {
            get
            {
                return _southWall;
            }
            private set
            {
                _southWall = value;
                RaisePropertyChanged("SouthWall");
            }
        }

        /// <summary>
        /// Gets or sets the west wall of the cell.
        /// </summary>
        public bool WestWall
        {
            get
            {
                return _leftWall;
            }
            private set
            {
                _leftWall = value;
                RaisePropertyChanged("WestWall");
            }
        }

        /// <summary>
        /// Gets or sets the state of the cell.
        /// </summary>
        public CellState CellState
        {
            get
            {
                return _cellState;
            }
            set
            {
                _cellState = value;
                RaisePropertyChanged("CellState");
            }
        }

        /// <summary>
        /// Gets or sets the cell type.
        /// </summary>
        public CellType CellType
        {
            get
            {
                return _cellType;
            }
            set
            {
                _cellType = value;
                RaisePropertyChanged("CellType");
            }
        }
        
        /// <summary>
        /// Gets or sets a flag indicating if the robot is inside this cell.
        /// </summary>
        public bool ContainsRobot
        {
            get
            {
                return _containsRobot;
            }
            set
            {
                _containsRobot = value;
                RaisePropertyChanged("ContainsRobot");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The RemoveWall method is called to remove a cell wall.
        /// </summary>
        /// <param name="cellWall"></param>
        public void RemoveWall(Direction cellWall)
        {
            try
            {
                switch (cellWall)
                {
                    case Direction.North:
                        NorthWall = false;
                        break;

                    case Direction.East:
                        EastWall = false;
                        break;

                    case Direction.South:
                        SouthWall = false;
                        break;

                    case Direction.West:
                        WestWall = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeCell.RemoveWall(CellWall cellWall): " + ex.ToString());
            }
        }

        /// <summary>
        /// The RestoreWall method is called to restore a cell wall.
        /// </summary>
        /// <param name="cellWall"></param>
        public void RestoreWall(Direction cellWall)
        {
            try
            {
                switch (cellWall)
                {
                    case Direction.North:
                        NorthWall = true;
                        break;

                    case Direction.East:
                        EastWall = true;
                        break;

                    case Direction.South:
                        SouthWall = true;
                        break;

                    case Direction.West:
                        WestWall = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MazeCell.RestoreWall(CellWall cellWall): " + ex.ToString());
            }
        }
        
        /// <summary>
        /// The ResetCell method is called to reset the cell.
        /// </summary>
        public void ResetCell()
        {
            NorthWall = true;
            EastWall = true;
            SouthWall = true;
            WestWall = true;
            CellState = CellState.Default;
            CellType = CellType.Normal;
        }
        
        #endregion
    }
}
