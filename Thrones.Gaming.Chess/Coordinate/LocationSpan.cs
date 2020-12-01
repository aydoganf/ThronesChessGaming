using System;
using System.Collections.Generic;
using System.Text;
using Thrones.Gaming.Chess.Movement;

namespace Thrones.Gaming.Chess.Coordinate
{
    public struct LocationSpan
    {
        public int XDiff { get; private set; }
        public int YDiff { get; private set; }
        public MovementDirection XMovement { get; private set; }
        public MovementDirection YMovement { get; private set; }
        public bool IsDiagonal { get; private set; }


        public LocationSpan(int xDiff, int yDiff, MovementDirection xMovement, MovementDirection yMovement) : this()
        {
            XDiff = xDiff;
            YDiff = yDiff;
            XMovement = xMovement;
            YMovement = yMovement;
            IsDiagonal = xDiff == yDiff;
        }
    }
}
