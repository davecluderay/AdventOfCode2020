using System;
using System.Linq;
using System.Numerics;

namespace Aoc2020_Day25
{
    internal class Solution
    {
        public string Title => "Day 25: Combo Breaker";

        public object PartOne()
        {
            var publicKeys = InputFile.ReadAllLines()
                                      .Select(s => Convert.ToInt64(s))
                                      .ToArray();

            var (matchedPublicKey, loopCount) = TransformUntilFirstMatch(7L, publicKeys);
            return Transform(publicKeys.Single(pk => pk != matchedPublicKey), loopCount);
        }

        public object PartTwo() => "That's it!";

        private static (long match, long loopCount) TransformUntilFirstMatch(long subjectNumber, long[] targets)
        {
            var current = subjectNumber;
            var loopCount = 0L;
            while (true)
            {
                loopCount++;
                current = Modulo(current * subjectNumber, 20201227);
                if (targets.Contains(current))
                    return (current, loopCount);
            }
        }

        private static long Transform(long subjectNumber, long loopCount)
        {
            var current = subjectNumber;
            for (var n = 0L;  n < loopCount; n++)
                current = Modulo(current * subjectNumber, 20201227);
            return current;
        }

        private static long Modulo(long number, int modulus)
        {
            var result = number % modulus;
            return result < 0 ? modulus + result : result;
        }
    }
}
