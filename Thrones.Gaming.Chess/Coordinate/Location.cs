using System;
using System.Collections.Generic;
using System.Text;
using Thrones.Gaming.Chess.Movement;

namespace Thrones.Gaming.Chess.Coordinate
{
    public class Location
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public string Name { get; private set; }


        public Location(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
        }

        public static LocationSpan operator -(Location target, Location current)
        {
            int xDiff = target.X - current.X;
            int yDiff = target.Y - current.Y;
            var xMovement = MovementDirection.None;
            var yMovement = MovementDirection.None;

            if (xDiff > 0)
            {
                xMovement = MovementDirection.Forward;
            }
            else if (xDiff < 0) 
            {
                xDiff *= -1;
                xMovement = MovementDirection.Backward;
            }

            if (yDiff > 0)
            {
                yMovement = MovementDirection.Forward;
            }
            else if (yDiff < 0) 
            { 
                yDiff *= -1;
                yMovement = MovementDirection.Backward;
            }

            return new LocationSpan(xDiff, yDiff, xMovement, yMovement);
        }
    }
}
