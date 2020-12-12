using System;
using System.Linq;

namespace Aoc2020_Day12
{
    internal class Solution
    {
        public string Title => "Day 12: Rain Risk";

        public object? PartOne()
        {
            var instructions = ReadInstructions();

            var position = (east: 0, north: 0);
            var orientation = (east: 1, north: 0);

            foreach (var (code, value) in instructions)
            {
                switch (code)
                {
                    case 'N':
                        position = (position.east, position.north + value);
                        break;
                    case 'S':
                        position = (position.east, position.north - value);
                        break;
                    case 'E':
                        position = (position.east + value, position.north);
                        break;
                    case 'W':
                        position = (position.east - value, position.north);
                        break;
                    case 'L':
                        orientation = RotateLeft(orientation, value);
                        break; 
                    case 'R': 
                        orientation = RotateRight(orientation, value);
                        break; 
                    case 'F':
                        position = (position.east + value * orientation.east,
                                    position.north + value * orientation.north);
                        break;
                }
            }
            
            return Math.Abs(position.east) + Math.Abs(position.north);
        }
        
        public object? PartTwo()
        {
            var instructions = ReadInstructions();

            var waypoint = (east: 10, north: 1);
            var position = (east: 0, north: 0);
            foreach (var (code, value) in instructions)
            {
                switch (code)
                {
                    case 'N':
                        waypoint = (waypoint.east, waypoint.north + value);
                        break;
                    case 'S':
                        waypoint = (waypoint.east, waypoint.north - value);
                        break;
                    case 'E':
                        waypoint = (waypoint.east + value, waypoint.north);
                        break;
                    case 'W':
                        waypoint = (waypoint.east - value, waypoint.north);
                        break;
                    case 'L':
                        waypoint = RotateLeft(waypoint, value);
                        break; 
                    case 'R': 
                        waypoint = RotateRight(waypoint, value);
                        break; 
                    case 'F':
                        position = (position.east + waypoint.east * value,
                                    position.north + waypoint.north * value);
                        break;
                }
            }
            
            return Math.Abs(position.east) + Math.Abs(position.north);
        }

        private static (int, int) RotateLeft((int, int) vector, int degrees)
        {
            return RotateRight(vector, 360 - degrees);
        }
        
        private static (int, int) RotateRight((int x, int y) vector, int degrees)
        {
            var turns = degrees % 360 / 90;

            for (var i = 0; i < turns; i++)
                vector = (vector.y, 0 - vector.x);

            return vector;
        }

        private static (char code, int value)[] ReadInstructions(string? fileName = null)
            => InputFile.ReadAllLines(fileName)
                        .Select(input => (input.First(), Convert.ToInt32(input.Substring(1))))
                        .ToArray();
    }
}