using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace adventofcode
{

    public class Day12 : IDay
    {
        private readonly List<string> _input;

        public Day12(List<string> input)
        {
            _input = input; 
        }

        public class Movement
        {
            public char Direction { get;set;}

            public int Distance { get;set;}
        }

        public class Ship
        {
            
            public Ship()
            {
                Movements = new List<Movement>();
            }

            public List<Movement> Movements { get;set; }

            public void Parse(List<string> inputs)
            {
                foreach(var input in inputs)
                {
                    Movements.Add(new Movement { Direction = input.Substring(0,1)[0], Distance = int.Parse(input.Substring(1,input.Length -1)) });
                }
            }

            public int Naviagate()
            {
                (int vertical, int horizontal) currentLocation = (0, 0);
                var currentDirection = 'E';

                foreach(var movement in Movements)
                {
                    switch(movement.Direction)
                    {
                        case 'N':
                        case 'E':
                        case 'S':
                        case 'W':
                            currentLocation = Move(currentLocation, movement.Direction, movement.Distance);
                            break;
                        case 'L':
                        case 'R':
                            currentDirection = GetNewDirection(currentDirection, movement.Direction, movement.Distance);
                            break;
                        case 'F':
                            currentLocation = Move(currentLocation, currentDirection, movement.Distance);
                            break;
                    }
                }

                return System.Math.Abs(currentLocation.vertical) + System.Math.Abs(currentLocation.horizontal);
            }

            public int NaviagateWayPoint()
            {
                (int vertical, int horizontal) currentLocation = (0, 0);
                (int vertical, int horizontal) currentWaypoint = (1, 10);

                foreach(var movement in Movements)
                {
                    switch(movement.Direction)
                    {
                        case 'N':
                        case 'E':
                        case 'S':
                        case 'W':
                            currentWaypoint = Move(currentWaypoint, movement.Direction, movement.Distance);
                            break;
                        case 'L':
                        case 'R':
                            currentWaypoint = RotateWaypoint(currentWaypoint, movement.Direction, movement.Distance);
                            break;
                        case 'F':
                            currentLocation = MoveToWaypoint(currentLocation, currentWaypoint, movement.Distance);
                            break;
                    }
                }

                return System.Math.Abs(currentLocation.vertical) + System.Math.Abs(currentLocation.horizontal);
            }


            private (int vertical, int horizontal) MoveToWaypoint((int vertical, int horizontal) currentLocation , (int vertical, int horizontal) currentWaypoint, int distance)
            {
                currentLocation.horizontal = currentLocation.horizontal + (distance * currentWaypoint.horizontal);
                currentLocation.vertical = currentLocation.vertical + (distance * currentWaypoint.vertical);

                return currentLocation;
            }

            private (int vertical, int horizontal) Move((int vertical, int horizontal) current ,char direction, int distance)
            {
                switch(direction)
                {
                    case 'N':
                        current.vertical += distance;
                        break;
                    case 'E':
                        current.horizontal += distance;
                        break;
                    case 'S':
                        current.vertical -= distance;
                        break;
                    case 'W':
                        current.horizontal -= distance;
                        break;
                }

                return current;
            }

            private (int vertical, int horizontal) RotateWaypoint((int vertical, int horizontal) currentWayPoint, char direction, int angle)
            {


                var turnCount = angle / 90;
            
                for(var turn = 0; turn < turnCount; turn++)
                {
                    (int vertical, int horizontal) newWaypoint = currentWayPoint;
                    var quadrant = 0;

                    if(currentWayPoint.horizontal >= 0 && currentWayPoint.vertical >= 0)
                    {
                        quadrant = 1;
                    }
                    if(currentWayPoint.horizontal >= 0 && currentWayPoint.vertical < 0)
                    {
                        quadrant = 2;
                    }
                    if(currentWayPoint.horizontal < 0 && currentWayPoint.vertical < 0)
                    {
                        quadrant = 3;
                    }
                    if(currentWayPoint.horizontal < 0 && currentWayPoint.vertical >= 0)
                    {
                        quadrant = 4;
                    }

                    switch(quadrant)
                    {
                        case 1:
                            if(direction == 'R')
                            {
                                newWaypoint.vertical = 0 - currentWayPoint.horizontal;
                                newWaypoint.horizontal = currentWayPoint.vertical;
                            }
                            else
                            {
                                newWaypoint.vertical = currentWayPoint.horizontal;
                                newWaypoint.horizontal = 0 - currentWayPoint.vertical;
                            }
                            break;
                        case 2:
                            if(direction == 'R')
                            {
                                newWaypoint.vertical = 0 - currentWayPoint.horizontal;
                                newWaypoint.horizontal = currentWayPoint.vertical;
                            }
                            else
                            {
                                newWaypoint.vertical = currentWayPoint.horizontal;
                                newWaypoint.horizontal = System.Math.Abs(currentWayPoint.vertical);
                            }
                            break;
                        case 3:
                            if(direction == 'R')
                            {
                                newWaypoint.vertical = System.Math.Abs(currentWayPoint.horizontal);
                                newWaypoint.horizontal = currentWayPoint.vertical;
                            }
                            else
                            {
                                newWaypoint.vertical = currentWayPoint.horizontal;
                                newWaypoint.horizontal = System.Math.Abs(currentWayPoint.vertical);
                            }
                            break;
                        case 4:
                            if(direction == 'R')
                            {
                                newWaypoint.vertical = System.Math.Abs(currentWayPoint.horizontal);
                                newWaypoint.horizontal = currentWayPoint.vertical;
                            }
                            else
                            {
                                newWaypoint.vertical = currentWayPoint.horizontal;
                                newWaypoint.horizontal = 0 - currentWayPoint.vertical;
                            }
                            break;
                        
                    }
                    currentWayPoint = newWaypoint;
                }

                return currentWayPoint;

            }

            private char GetNewDirection(char current, char direction, int angle)
            {
                var turnCount = angle / 90;

                var directionRight = new Dictionary<char,char>();
                directionRight.Add('N','E');
                directionRight.Add('E','S');
                directionRight.Add('S','W');
                directionRight.Add('W','N');

                var directionLeft = new Dictionary<char,char>();
                directionLeft.Add('N','W');
                directionLeft.Add('E','N');
                directionLeft.Add('S','E');
                directionLeft.Add('W','S');

                var directions = direction == 'L' ? directionLeft : directionRight;

                var newDirection = current;

                for(var turn = 0; turn < turnCount; turn++)
                {
                    newDirection = directions[newDirection];
                }

                return newDirection;

            }

        }

        public string ChallengeOne()
        {
            var ship = new Ship();
            ship.Parse(_input);
            return ship.Naviagate().ToString();
        }

        public string ChallengeTwo()
        {
            var ship = new Ship();
            ship.Parse(_input);
            return ship.NaviagateWayPoint().ToString();
        }
    }
}