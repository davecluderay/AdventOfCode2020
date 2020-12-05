using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day04
{
    internal class Solution
    {
        public string Title => "Day 4: Passport Processing";

        public object PartOne()
        {
            return ReadPassports().Count(p => IsValidPassport(p, false));
        }

        public object PartTwo()
        {
            return ReadPassports().Count(p => IsValidPassport(p, true));
        }

        private static (string key, string val)[][] ReadPassports(string fileName = null)
            => Regex.Split(InputFile.ReadAllText(fileName), @"\r?\n\r?\n", RegexOptions.Multiline)
                    .Select(ParsePassport).ToArray();

        private static (string key, string val)[] ParsePassport(string text)
            => (from token in Regex.Split(text, @"\s", RegexOptions.Multiline)
                where !string.IsNullOrEmpty(token)
                let split = token.Split(':', 2)
                select (key: split[0], val: split[1])).ToArray();

        private static bool IsValidPassport((string key, string val)[] passport, bool full)
        {
            return RequiredFields.All(r => passport.Any(f => f.key == r.key && (!full || r.validator(f.val))));
        }

        private static bool IsValidNumeric(string value, int min, int max)
            => int.TryParse(value, out var parsed) && parsed >= min && parsed <= max;

        private static bool IsValidPattern(string value, string pattern)
            => Regex.IsMatch(value, pattern);

        private static bool IsValidHeight(string value)
        {
            var heightMatch = new Regex(@"(?<num>\d+)(?<unit>in|cm)").Match(value);
            if (!heightMatch.Success) return false;

            var (num, unit) = (Convert.ToInt64(heightMatch.Groups["num"].Value), heightMatch.Groups["unit"].Value);

            var (min, max) = unit == "cm"
                ? (min: 150, max: 193)
                : (min: 59, max: 76);
            return num >= min && num <= max;
        }

        private static readonly (string key, Func<string, bool> validator)[] RequiredFields =
        {
            ( "byr", val => IsValidNumeric(val, 1920, 2002)                         ), // (Birth Year)
            ( "iyr", val => IsValidNumeric(val, 2010, 2020)                         ), // (Issue Year)
            ( "eyr", val => IsValidNumeric(val, 2020, 2030)                         ), // (Expiration Year)
            ( "hgt", val => IsValidHeight(val)                                      ), // (Height)
            ( "hcl", val => IsValidPattern(val, @"^#[0-9a-f]{6}$")                  ), // (Hair Color)
            ( "ecl", val => IsValidPattern(val, @"^(amb|blu|brn|gry|grn|hzl|oth)$") ), // (Eye Color)
            ( "pid", val => IsValidPattern(val, @"^\d{9}$")                         )  // (Passport ID)
        };
    }
}