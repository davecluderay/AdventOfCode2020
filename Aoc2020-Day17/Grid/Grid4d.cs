using System;
using System.Collections.Generic;

namespace Aoc2020_Day17
{
    internal class Grid4d : Grid<(int x, int y, int z, int w)>
    {
        protected override ((int x, int y, int z, int w) min, (int x, int y, int z, int w) max) AddToBounds(((int x, int y, int z, int w) min, (int x, int y, int z, int w) max) bounds, (int x, int y, int z, int w) position)
        {
            var minX = Math.Min(bounds.min.x, position.x);
            var minY = Math.Min(bounds.min.y, position.y);
            var minZ = Math.Min(bounds.min.z, position.z);
            var minW = Math.Min(bounds.min.w, position.w);
            var maxX = Math.Max(bounds.max.x, position.x);
            var maxY = Math.Max(bounds.max.y, position.y);
            var maxZ = Math.Max(bounds.max.z, position.z);
            var maxW = Math.Max(bounds.max.w, position.w);
            return ((minX, minY, minZ, minW), (maxX, maxY, maxZ, maxW));
        }

        protected override ((int x, int y, int z, int w) min, (int x, int y, int z, int w) max) Expand(((int x, int y, int z, int w) min, (int x, int y, int z, int w) max) bounds, int amount = 1)
            => ((x: bounds.min.x - amount, y: bounds.min.y - amount, z: bounds.min.z - amount, w: bounds.min.w - amount),
                (x: bounds.max.x + amount, y: bounds.max.y + amount, z: bounds.max.z + amount, w: bounds.max.w + amount));

        protected override IEnumerable<(int x, int y, int z, int w)> GetPositionsInsideBounds(((int x, int y, int z, int w) min, (int x, int y, int z, int w) max) bounds)
        {
            for (var x = bounds.min.x; x <= bounds.max.x; x++)
            for (var y = bounds.min.y; y <= bounds.max.y; y++)
            for (var z = bounds.min.z; z <= bounds.max.z; z++)
            for (var w = bounds.min.w; w <= bounds.max.w; w++)
                yield return (x, y, z, w);
        }

        public static Grid4d LoadInitialState(string? fileName = null)
        {
            var lines = InputFile.ReadAllLines();

            var grid = new Grid4d();

            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[y].Length; x++)
            {
                grid.Set((x, y, 0, 0), lines[y][x]);
            }

            return grid;
        }
    }
}