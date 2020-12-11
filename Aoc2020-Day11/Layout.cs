using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day11
{
    internal class Layout
    {
        private static readonly (int row, int column)[] DirectionalVectors = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

        private const char OccupiedSeat = '#';
        private const char EmptySeat = 'L';
        private const char EmptySpace = '.';
        
        private readonly char[,] _data;

        public int Columns { get; }
        public int Rows { get; }

        public Layout(string[] lines)
        {
            Rows = lines.Length;
            Columns = lines[0].Length;
            _data = new char[Rows, Columns];

            foreach (var position in GetAllPositions())
            {
                _data[position.row, position.column] = lines[position.row][position.column];
            }
        }

        public bool IsOccupiedSeatAt((int x, int y) position) => GetAt(position) == OccupiedSeat;
        public bool IsEmptySeatAt((int row, int column) position) => GetAt(position) == EmptySeat;
        public bool IsEmptySpaceAt((int row, int column) position) => GetAt(position) == EmptySpace;
        public int AdjacentOccupiedSeatCount((int row, int column) to) => GetAdjacentPositions(to).Count(IsOccupiedSeatAt);
        public int VisibleOccupiedSeatCount((int row, int column) to) => GetVisibleSeatPositions(to).Count(IsOccupiedSeatAt);
        
        public void SetOccupiedSeatAt((int x, int y) position) => SetAt(position, OccupiedSeat);
        public void SetEmptySeatAt((int x, int y) position) => SetAt(position, EmptySeat);
        public void SetEmptySpaceAt((int x, int y) position) => SetAt(position, EmptySpace);

        public IEnumerable<(int row, int column)> AllSeatPositions
            => GetAllPositions().Where(p => !IsEmptySpaceAt(p));

        private IEnumerable<(int row, int column)> GetAllPositions()
        {
            for (var row = 0; row < Rows; row++)
            for (var column = 0; column < Columns; column++)
            {
                yield return (row, column);
            }
        }

        private IEnumerable<(int row, int column)> GetAdjacentPositions((int row, int column) to)
            => DirectionalVectors.Select(v => (row: to.row + v.row, column: to.column + v.column))
                                 .Where(IsInBounds)
                                 .ToArray();

        private IEnumerable<(int row, int column)> GetVisibleSeatPositions((int row, int column) to)
        {
            foreach (var vec in DirectionalVectors)
            {
                var pos = (row: to.row + vec.row, column: to.column + vec.column);
                while (IsInBounds(pos) && IsEmptySpaceAt(pos))
                    pos = (pos.row + vec.row, pos.column + vec.column);
                if (IsInBounds(pos)) yield return pos;
            }
        }

        private char GetAt((int row, int column) position)
            => _data[position.row, position.column];

        private void SetAt((int row, int column) position, char @char)
            => _data[position.row, position.column] = @char;

        private bool IsInBounds((int row, int column) pos)
            => pos.row >= 0 && pos.column >= 0 && pos.row < Rows && pos.column < Columns;
    }
}