using System.Linq;

namespace Aoc2020_Day05
{
    internal class Solution
    {
        public string Title => "Day 5: Binary Boarding";

        public object? PartOne()
        {
            return InputFile.ReadAllLines()
                            .Max(DecodeBoardingPass);
        }

        public object? PartTwo()
        {
            var seatNumbers = InputFile.ReadAllLines()
                                       .Select(DecodeBoardingPass)
                                       .OrderBy(x => x)
                                       .ToArray();

            var first = seatNumbers.First();
            for (var i = 0; i < seatNumbers.Length; i++)
            {
                var expected = first + i;
                if (seatNumbers[i] != expected) return expected;
            }

            return null;
        }

        private int DecodeBoardingPass(string code)
        {
            // The code is just the binary representation of the seat number.
            // e.g. BFBBFFF LRR" -> 1011000 011
            var result = 0;
            for (var bit = 0; bit < 10; bit++)
                if (code[9 - bit] == 'B' || code[9 - bit] == 'R') result |= 1 << bit;
            return result;
        }
    }
}