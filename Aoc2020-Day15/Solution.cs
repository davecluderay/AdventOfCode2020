using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day15
{
    internal class Solution
    {
        public string Title => "Day 15: Rambunctious Recitation";

        public object PartOne()
        {
            return Solve(2020L);
        }

        public object PartTwo()
        {
            // TODO: Calculating this many values takes maybe 10 seconds.
            // Is it possible to derive a formula for the value at position n?
            return Solve(30000000L);
        }

        private long Solve(long targetTurn, string? fileName = null)
        {
            var starters = ReadStarterNumbers(fileName);
            var history = starters.Select((n, i) => (n, i))
                                  .ToDictionary(x => x.n, x => new RecentHistory<long>(2, new[] { (long) x.i }));
            var last = starters[^1];

            for (var turn = starters.Length; turn < targetTurn; turn++)
            {
                var previous = Get(history, last);
                var number = previous.IsFull
                    ? previous.Get(1) - previous.Get(0)
                    : 0L;
                Add(history, number, turn);
                last = number;
            }

            return last;
        }

        private static long[] ReadStarterNumbers(string? fileName = null)
            => InputFile.ReadAllLines(fileName)
                        .SelectMany(l => l.Split(',', StringSplitOptions.RemoveEmptyEntries))
                        .Select(l => Convert.ToInt64(l))
                        .ToArray();

        private static RecentHistory<long> Get(IDictionary<long, RecentHistory<long>> history, long number)
        {
            if (!history.ContainsKey(number))
                history[number] = new RecentHistory<long>(2);
            return history[number];
        }

        private static void Add(IDictionary<long, RecentHistory<long>> history, long number, long turn)
            => Get(history, number).Record(turn);
    }
}
