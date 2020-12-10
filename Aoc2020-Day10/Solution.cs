using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day10
{
    internal class Solution
    {
        public string Title => "Day 10: Adapter Array";

        public object PartOne()
        {
            var joltages = LoadJoltages();

            var diffs = new List<int>(joltages.Length);
            for (var i = 1; i < joltages.Length; i++)
            {
                var rating = joltages[i];
                diffs.Add(rating - joltages[i - 1]);
            }

            return diffs.Count(d => d == 1) * diffs.Count(d => d == 3);
        }
        
        public object PartTwo()
        {
            var joltages = LoadJoltages();

            var solutionCountsByPosition = new Dictionary<int, long>();
            long CountSolutionsFrom(int position)
            {
                // If this position was already evaluated, return the cached result.
                if (solutionCountsByPosition.ContainsKey(position)) return solutionCountsByPosition[position];

                long count = 0;

                if (position == joltages.Length - 1)
                {
                    // It is the last position in the sequence,
                    // so there is only the one solution from here.
                    count = 1; 
                }
                else
                {
                    // There are potentially several next position choices from here,
                    // so evaluate each one recursively.
                    var next = position + 1;
                    while (next < joltages.Length && joltages[next] - joltages[position] < 4)
                        count += CountSolutionsFrom(next++);
                }

                // Cache and return the number of available solutions
                // from this position in the sequence.
                solutionCountsByPosition[position] = count;
                return count;
            }

            return CountSolutionsFrom(0);
        }

        private int[] LoadJoltages(string fileName = null)
        {
            var joltages = new List<int>();
            joltages.Add(0);
            joltages.AddRange(InputFile.ReadAllLines(fileName)
                                       .Select(l => Convert.ToInt32(l))
                                       .OrderBy(l => l));
            joltages.Add(joltages.Last() + 3);
            return joltages.ToArray();
        }
    }
}