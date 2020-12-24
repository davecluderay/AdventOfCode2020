using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day24
{
    internal class Solution
    {
        public string Title => "Day 24: Lobby Layout";

        public object PartOne()
        {
            var blackTilePositions = FindInitialBlackTilePositions();
            return blackTilePositions.Length;
        }

        public object PartTwo()
        {
            var blackTilePositions = FindInitialBlackTilePositions();
            blackTilePositions = SimulateExhibit(blackTilePositions, numberOfDays: 100);
            return blackTilePositions.Length;
        }

        private static (int x, int y)[] SimulateExhibit((int x, int y)[] initialBlackTilePositions, int numberOfDays)
        {
            var floor = new TiledFloor(initialBlackTilePositions);

            for (var day = 0; day < numberOfDays; day++)
            {
                var actions = new List<Action>();

                actions.AddRange(floor.GetBlackTilePositions()
                                      .Select(p => (position: p,
                                                    adjacentBlackTiles: floor.CountAdjacentBlackTiles(p)))
                                      .Where(p => p.adjacentBlackTiles == 0 || p.adjacentBlackTiles > 2)
                                      .Select(p => (Action)(() => floor.Set(p.position, Tile.White))));

                var whiteTilesAdjacentToBlackTiles = floor.GetBlackTilePositions()
                                                          .SelectMany(p => floor.GetAdjacentWhiteTilePositions(p))
                                                          .Distinct();
                actions.AddRange(whiteTilesAdjacentToBlackTiles.Where(p => floor.CountAdjacentBlackTiles(p) == 2)
                                                               .Select(p => (Action)(() => floor.Set(p, Tile.Black))));

                foreach (var action in actions)
                    action.Invoke();
            }

            return floor.GetBlackTilePositions();
        }

        private static (int x, int y)[] FindInitialBlackTilePositions(string? fileName = null)
            => ReadInstructionSets(fileName)
               .Select(instructionSet => ApplyInstructions(instructionSet, (x: 0, y: 0)))
               .GroupBy(x => x)
               .Where(g => g.Count() % 2 == 1)
               .Select(g => g.Key)
               .ToArray();

        private static string[][] ReadInstructionSets(string? fileName = null)
        {
            var pattern = new Regex("([ns][ew]|(?<![ns])[ew])", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            return InputFile.ReadAllLines(fileName)
                            .Select(l => pattern.Matches(l)
                                                .Select(m => m.Value)
                                                .ToArray())
                            .ToArray();
        }

        private static (int x, int y) ApplyInstructions(string[] instructionSet, (int x, int y) start)
            => instructionSet.Aggregate(
                start,
                (current, instruction) => instruction switch
                                          {
                                              "nw" => (current.x - 1, current.y - 1),
                                              "ne" => (current.x + 1, current.y - 1),
                                              "e"  => (current.x + 2, current.y),
                                              "se" => (current.x + 1, current.y + 1),
                                              "sw" => (current.x - 1, current.y + 1),
                                              "w"  => (current.x - 2, current.y),
                                              _ => throw new ArgumentException($"Unrecognised instruction: {instruction}",
                                                                               nameof(instructionSet))
                                          });
    }
}
