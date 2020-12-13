using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime;

namespace Aoc2020_Day13
{
    internal class Solution
    {
        public string Title => "Day 13: Shuttle Search";

        public object? PartOne()
        {
            var (start, schedule) = ReadSchedule();
            var earliestBus = schedule.OrderBy(s => Calculate.Modulo(s.bus - start, s.bus))
                                      .Select(s => s.bus)
                                      .First();
            return earliestBus * Calculate.Modulo(earliestBus - start, earliestBus);
        }

        public object? PartTwo()
        {
            var (_, schedule) = ReadSchedule();

            // Build a function: fn(x) => ax + b
            // The function defines the timestamps where buses will arrive at exactly the required times.
            // Recalculate the constants a and b for each bus in the schedule.
            var fn = (a: 1L, b: 0L);
            foreach (var x in schedule)
            {
                var time = fn.b;
                while (Calculate.Modulo(time + x.position, x.bus) != 0) // iterate until the bus arrive the correct amount of time from t.
                    time += fn.a;

                fn = (Calculate.LowestCommonMultiple(fn.a, x.bus), time);
            }

            return fn.b;
        }

        private static (long departFrom, (long bus, long position)[] schedule) ReadSchedule(string? fileName = null)
        {
            var lines = InputFile.ReadAllLines(fileName);
            var departFrom = long.Parse(lines[0]);
            var schedule = lines[1].Split(',')
                                   .Select((v, i) => (bus: v, position: i))
                                   .Where(x => x.bus != "x")
                                   .Select(x => (bus: long.Parse(x.bus), position: (long) x.position))
                                   .ToArray();

            return (departFrom, schedule);
        }
    }
}
