using System;
using System.Collections.Generic;

namespace Aoc2020_Day17
{
    internal class Solution
    {
        public string Title => "Day 17: Conway Cubes";

        public object PartOne()
        {
            var grid = Grid3d.LoadInitialState();
            return RunSimulation(grid);
        }

        public object PartTwo()
        {
            var grid = Grid4d.LoadInitialState();
            return RunSimulation(grid);
        }

        private static long RunSimulation<T>(Grid<T> grid, int cycles = 6) where T : notnull
        {
            for (var cycle = 0; cycle < cycles; cycle++)
            {
                var actions = new List<Action>();
                foreach (var position in grid.GetAllPotentialPositionsForNextCycle())
                {
                    if (grid.IsActive(position))
                    {
                        var activeNeighbours = grid.CountActiveNeighbours(position);
                        if (activeNeighbours < 2 || activeNeighbours > 3)
                            actions.Add(() => grid.Set(position, Grid.InactiveState));
                    }
                    else
                    {
                        var activeNeighbours = grid.CountActiveNeighbours(position);
                        if (activeNeighbours == 3)
                            actions.Add(() => grid.Set(position, Grid.ActiveState));
                    }
                }

                foreach (var action in actions)
                    action.Invoke();
            }

            return grid.CountActivePositions();
        }
    }
}
