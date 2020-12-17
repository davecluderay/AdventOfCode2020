using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day17
{
    internal class Grid3d : Grid<(int x, int y, int z)>
    {
        protected override ((int x, int y, int z) min, (int x, int y, int z) max) AddToBounds(((int x, int y, int z) min, (int x, int y, int z) max) bounds, (int x, int y, int z) position)
        {
            var minX = Math.Min(bounds.min.x, position.x);
            var minY = Math.Min(bounds.min.y, position.y);
            var minZ = Math.Min(bounds.min.z, position.z);
            var maxX = Math.Max(bounds.max.x, position.x);
            var maxY = Math.Max(bounds.max.y, position.y);
            var maxZ = Math.Max(bounds.max.z, position.z);
            return ((minX, minY, minZ), (maxX, maxY, maxZ));
        }

        protected override ((int x, int y, int z) min, (int x, int y, int z) max) Expand(((int x, int y, int z) min, (int x, int y, int z) max) bounds, int amount = 1)
            => ((x: bounds.min.x - amount, y: bounds.min.y - amount, z: bounds.min.z - amount),
                (x: bounds.max.x + amount, y: bounds.max.y + amount, z: bounds.max.z + amount));

        protected override IEnumerable<(int x, int y, int z)> GetPositionsInsideBounds(((int x, int y, int z) min, (int x, int y, int z) max) bounds)
        {
            for (var x = bounds.min.x; x <= bounds.max.x; x++)
            for (var y = bounds.min.y; y <= bounds.max.y; y++)
            for (var z = bounds.min.z; z <= bounds.max.z; z++)
                yield return (x, y, z);
        }

        public static Grid3d LoadInitialState(string? fileName = null)
        {
            var lines = InputFile.ReadAllLines();

            var grid = new Grid3d();

            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[y].Length; x++)
            {
                grid.Set((x, y, 0), lines[y][x]);
            }

            return grid;
        }
    }
}
