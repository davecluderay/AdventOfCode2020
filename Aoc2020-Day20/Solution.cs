using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day20
{
    internal class Solution
    {
        public string Title => "Day 20: Jurassic Jigsaw";

        public object PartOne()
        {
            var (cornerTiles, _, _) = ReadAndCategoriseTiles();
            return cornerTiles.Aggregate(1L, (product, tile) => tile.Id * product);
        }

        public object? PartTwo()
        {
            var tiles = ReadAndCategoriseTiles();

            var mapBuilder = new MapBuilder();
            var map = mapBuilder.ReconstructMap(tiles.CornerTiles
                                                     .Concat(tiles.EdgeTiles)
                                                     .Concat(tiles.InnerTiles));

            var mapInspector = new MapInspector();
            var updatedMap = mapInspector.RevealSeaMonsters(map);

            return updatedMap.Count(@char => @char == '#');
        }

        private (Tile[] CornerTiles, Tile[] EdgeTiles, Tile[] InnerTiles) ReadAndCategoriseTiles(string? fileName = null)
        {
            var tiles = ReadTiles(fileName);
            return CategoriseTiles(tiles);
        }

        private Tile[] ReadTiles(string? fileName = null)
        {
            return InputFile.ReadAllLines(fileName)
                            .InSections()
                            .Select(s => Tile.Read(s))
                            .ToArray();
        }

        private (Tile[] CornerTiles, Tile[] EdgeTiles, Tile[] InnerTiles) CategoriseTiles(Tile[] tiles)
        {
            var examinedTiles = tiles.Select(t => (Tile: t, Count: CountMatchedBorders(tiles, t)))
                                     .ToLookup(t => t.Count, t => t.Tile);
            return (examinedTiles[2].ToArray(),
                    examinedTiles[3].ToArray(),
                    examinedTiles[4].ToArray());
        }

        private int CountMatchedBorders(IEnumerable<Tile> allTiles, Tile tile)
        {
            var otherTiles = allTiles.Except(new[] { tile }).ToArray();

            var count = 0;
            foreach (var border in tile.Borders)
            {
                if (otherTiles.Any(o => o.Borders.Any(b => b == border ||
                                                           b == border.Reverse())))
                    count++;
            }

            return count;
        }
    }
}
