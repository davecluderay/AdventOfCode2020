using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day17
{
    internal class MultiDimensionalGrid : Grid<MultiDimensionalGridPosition>
    {
        private readonly int _dimensions;

        private MultiDimensionalGrid(int dimensions)
        {
            _dimensions = dimensions;
        }

        protected override (MultiDimensionalGridPosition min, MultiDimensionalGridPosition max) AddToBounds((MultiDimensionalGridPosition min, MultiDimensionalGridPosition max) bounds, MultiDimensionalGridPosition position)
        {
            var minimums = new long[_dimensions];
            var maximums = new long[_dimensions];
            for (var dimension = 0; dimension < _dimensions; dimension++)
            {
                minimums[dimension] = Math.Min(bounds.min.Values[dimension], position.Values[dimension]);
                maximums[dimension] = Math.Max(bounds.max.Values[dimension], position.Values[dimension]);
            }
            return (new MultiDimensionalGridPosition(minimums), new MultiDimensionalGridPosition(maximums));
        }

        protected override (MultiDimensionalGridPosition min, MultiDimensionalGridPosition max) Expand((MultiDimensionalGridPosition min, MultiDimensionalGridPosition max) bounds, int amount = 1)
            => (new MultiDimensionalGridPosition(bounds.min.Values.Select(p => p - amount).ToArray()),
                new MultiDimensionalGridPosition(bounds.max.Values.Select(p => p + amount).ToArray()));

        protected override IEnumerable<MultiDimensionalGridPosition> GetPositionsInsideBounds((MultiDimensionalGridPosition min, MultiDimensionalGridPosition max) bounds)
        {
            var (minValues, maxValues) = (bounds.min.Values, bounds.max.Values);
            var current = minValues.ToArray();

            var done = false;
            while (!done)
            {
                yield return new MultiDimensionalGridPosition(current);

                for (var index = _dimensions - 1; index >= 0; index--)
                {
                    if (current[index] == maxValues[index])
                    {
                        current[index] = minValues[index];
                        if (index == 0)
                        {
                            done = true;
                            break;
                        }
                    }
                    else
                    {
                        current[index] = current[index] + 1;
                        break;
                    }
                }
            }
        }

        public static MultiDimensionalGrid LoadInitialState(int dimensions, string? fileName = null)
        {
            var lines = InputFile.ReadAllLines();

            var grid = new MultiDimensionalGrid(dimensions);

            var values = new long[dimensions];

            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[y].Length; x++)
            {
                values[0] = x;
                values[1] = y;
                grid.Set(new MultiDimensionalGridPosition(values), lines[y][x]);
            }

            return grid;
        }
    }
}
