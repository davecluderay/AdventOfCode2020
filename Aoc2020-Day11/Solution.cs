using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day11
{
    internal class Solution
    {
        public string Title => "Day 11: Seating System";

        public object PartOne()
        {
            var layout = new Layout(InputFile.ReadAllLines());

            while (true)
            {
                var changes = new List<Action>();

                foreach (var position in layout.AllSeatPositions)
                {
                    var isOccupied = layout.IsOccupiedSeatAt(position);
                    var adjacentOccupiedCount = layout.AdjacentOccupiedSeatCount(position);

                    if (!isOccupied && adjacentOccupiedCount == 0)
                        changes.Add(() => layout.SetOccupiedSeatAt(position));

                    if (isOccupied && adjacentOccupiedCount >= 4)
                        changes.Add(() => layout.SetEmptySeatAt(position));
                }

                foreach (var change in changes)
                    change.Invoke();

                if (!changes.Any()) break;
            }

            return layout.AllSeatPositions.Count(p => layout.IsOccupiedSeatAt(p));
        }
        
        public object PartTwo()
        {
            var layout = new Layout(InputFile.ReadAllLines());

            while (true)
            {
                var changes = new List<Action>();

                foreach (var position in layout.AllSeatPositions)
                {
                    var isOccupied = layout.IsOccupiedSeatAt(position);
                    var visibleOccupiedCount = layout.VisibleOccupiedSeatCount(position);

                    if (!isOccupied && visibleOccupiedCount == 0)
                        changes.Add(() => layout.SetOccupiedSeatAt(position));

                    if (isOccupied && visibleOccupiedCount >= 5)
                        changes.Add(() => layout.SetEmptySeatAt(position));
                }

                foreach (var change in changes)
                    change.Invoke();

                if (!changes.Any()) break;
            }

            return layout.AllSeatPositions.Count(p => layout.IsOccupiedSeatAt(p));
        }
    }
}