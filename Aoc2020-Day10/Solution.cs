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

            var solutionCounts = new long[joltages.Length];
            solutionCounts[^1] = 1; 
            long CountSolutionsFrom(int position)
            {
                // If the solution count from this position is known, return it.
                if (solutionCounts[position] != 0) return solutionCounts[position];

                // Compute the solution count from this position recursively.
                var next = position + 1;
                while (next < joltages.Length && joltages[next] - joltages[position] < 4)
                    solutionCounts[position] += CountSolutionsFrom(next++);

                // Return it.
                return solutionCounts[position];
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