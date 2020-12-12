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
                (position, orientation) = code switch
                {
                    'N' => ((position.east, position.north + value), orientation),
                    'S' => ((position.east, position.north - value), orientation),
                    'E' => ((position.east + value, position.north), orientation),
                    'W' => ((position.east - value, position.north), orientation),
                    'L' => (position, RotateLeft(orientation, value)),
                    'R' => (position, RotateRight(orientation, value)),
                    'F' => ((position.east + value * orientation.east, position.north + value * orientation.north), orientation),
                    _ => throw new NotSupportedException()
                };
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
                (position, waypoint) = code switch
                {
                    'N' => (position, (waypoint.east, waypoint.north + value)),
                    'S' => (position, (waypoint.east, waypoint.north - value)),
                    'E' => (position, (waypoint.east + value, waypoint.north)),
                    'W' => (position, (waypoint.east - value, waypoint.north)),
                    'L' => (position, RotateLeft(waypoint, value)),
                    'R' => (position, RotateRight(waypoint, value)),
                    'F' => ((position.east + value * waypoint.east, position.north + value * waypoint.north), waypoint),
                    _ => throw new NotSupportedException()
                };
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