using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day06
{
    internal class Solution
    {
        public string Title => "Day 6: Custom Customs";

        public object? PartOne()
        {
            return ReadGroups().Select(g => g.SelectMany(s => s.ToCharArray())
                                             .Distinct()
                                             .Count())
                               .Sum();
        }
        
        public object? PartTwo()
        {
            var questions = Enumerable.Range('a', 26).Select(n => (char)n).ToArray();
            return ReadGroups().Select(g => questions.Count(c => g.All(l => l.Contains(c))))
                               .Sum();
        }

        private static IEnumerable<string[]> ReadGroups(string? fileName = null)
        {
            var groups = Regex.Split(InputFile.ReadAllText(fileName).Trim(),
                                     @"\r?\n\r?\n",
                                     RegexOptions.Multiline);
            return groups.Select(g => Regex.Split(g,
                                                  @"\r?\n",
                                                  RegexOptions.Multiline));
        }
    }
}