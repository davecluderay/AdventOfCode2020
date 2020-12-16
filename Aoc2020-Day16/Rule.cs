using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day16
{
    internal class Rule
    {
        private static readonly Regex RulePattern = new Regex(@"^(?<fieldName>.*):\s*((?<low>\d+)-(?<high>\d+)(\s+or\s+)?)+$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private readonly (int Low, int High)[] _ranges;

        public string FieldName { get; }

        private Rule(string fieldName, (int Low, int High)[] ranges)
        {
            FieldName = fieldName;
            _ranges = ranges;
        }

        public bool IsValid(int value)
        {
            return _ranges.Any(range => value >= range.Low && value <= range.High);
        }

        public static Rule Parse(string text)
        {
            var match = RulePattern.Match(text);
            if (!match.Success) throw new ArgumentException($"Bad rule format: {text}", nameof(text));

            var fieldName = match.Groups["fieldName"].Value;
            var ranges = Enumerable.Range(0, match.Groups["low"].Captures.Count)
                .Select(i => (Convert.ToInt32(match.Groups["low"].Captures[i].Value),
                    Convert.ToInt32(match.Groups["high"].Captures[i].Value)))
                .ToArray();
            return new Rule(fieldName, ranges);
        }
    }
}
