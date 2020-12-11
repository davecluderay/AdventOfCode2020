using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day02
{
    internal class Solution
    {
        public string Title => "Day 2: Password Philosophy";

        public object? PartOne()
        {
            var matches = ReadPasswords();
            var validPasswords =
                from m in matches
                let n = m.password?.Count(c => c == m.@char)
                where n >= m.low && n <= m.high
                select m;
            return validPasswords.Count();
        }

        public object? PartTwo()
        {
            var matches = ReadPasswords();
            var validPasswords =
                from m in matches
                let c1 = m.password?[m.low - 1]
                let c2 = m.password?[m.high - 1]
                where (c1 == m.@char || c2 == m.@char) && c1 != c2
                select m;
            return validPasswords.Count();
        }

        private static (int low, int high, char @char, string password)[] ReadPasswords(string? fileName = null)
        {
            var pattern = new Regex("^(?<low>\\d+)-(?<high>\\d+)\\s+(?<char>.):\\s+(?<password>.*)$", RegexOptions.Compiled);

            return InputFile.ReadAllLines(fileName)
                .Select(l => pattern.Match(l))
                .Select(m => (Convert.ToInt32(m.Groups["low"].Value),
                              Convert.ToInt32(m.Groups["high"].Value),
                              m.Groups["char"].Value.Single(),
                              m.Groups["password"].Value))
                .ToArray();
        }
    }
}