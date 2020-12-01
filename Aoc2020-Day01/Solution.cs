using System;
using System.Linq;

namespace Aoc2020_Day01
{
    internal class Solution
    {
        private const int ExpectSum = 2020;

        public string Title => "Day 1: Report Repair";

        public object PartOne()
        {
            var entries = ReadEntries();

            for (var i = 0     ; i < entries.Length - 1 && i < ExpectSum / 2 ; i++)
            for (var j = i + 1 ; j < entries.Length                          ; j++)
            {
                var (x, y) = (entries[i], entries[j]);
                var sum = x + y;
                if (sum == ExpectSum) return x * y;
                if (sum > ExpectSum) break;
            }

            return null;
        }

        public object PartTwo()
        {
            var entries = ReadEntries();

            for (var i = 0     ; i < entries.Length - 2 && i     < ExpectSum / 2 ; i++)
            for (var j = i + 1 ; j < entries.Length - 1 && i + j < ExpectSum / 2 ; j++)
            for (var k = j + 1 ; k < entries.Length                              ; k++)
            {
                var (x, y, z) = (entries[i], entries[j], entries[k]);
                var sum = x + y + z;
                if (sum == ExpectSum) return x * y * z;
                if (sum > ExpectSum) break;
            }

            return null;
        }

        private int[] ReadEntries(string fileName = null) =>
            InputFile.ReadAllLines(fileName)
                .Select(l => Convert.ToInt32(l))
                .OrderBy(n => n)
                .ToArray();
    }
}