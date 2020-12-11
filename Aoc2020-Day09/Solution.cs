using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day09
{
    internal class Solution
    {
        const int PreambleLength = 25;
        
        public string Title => "Day 9: Encoding Error";

        public object? PartOne()
        {
            var sequence = ReadSequence();
            
            var buffer = sequence.Take(PreambleLength).ToArray();
            var nextPos = 0;

            foreach (var nextValue in sequence.Skip(buffer.Length))
            {
                if (InPairs(buffer).All(pair => pair.a + pair.b != nextValue))
                    return nextValue;

                buffer[nextPos] = nextValue;
                nextPos = ++nextPos % buffer.Length;
            }

            return null;
        }

        public object? PartTwo()
        {
            var sequence = ReadSequence();

            var target = Convert.ToInt64(PartOne());

            for (int skip = 0; skip < sequence.Length - 1; skip++)
            for (var take = 2; take < sequence.Length - skip; take++)
            {
                var values = sequence.Skip(skip).Take(take).ToArray();
                var sum = values.Sum();
                
                if (sum > target) break;
                if (sum == target) return values.Min() + values.Max();
            }

            return null;
        }

        private static long[] ReadSequence(string? fileName = null)
            => InputFile.ReadAllLines(fileName)
                .Select(s => Convert.ToInt64(s))
                .ToArray();

        private IEnumerable<(long a, long b)> InPairs(long[] values)
        {
            for (var ax = 0; ax < values.Length -1; ax++)
            for (var bx = ax + 1; bx < values.Length; bx++)
                yield return (values[ax], values[bx]);
        }
    }
}