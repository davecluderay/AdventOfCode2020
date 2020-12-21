using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day20
{
    internal static class Extensions
    {
        public static string Reverse(this string text)
        {
            return new string(((IEnumerable<char>)text).Reverse().ToArray());
        }

        public static IEnumerable<string[]> InSections(this IEnumerable<string> text)
        {
            var section = new List<string>();
            foreach (var line in text)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (section.Count > 0)
                    {
                        yield return section.ToArray();
                        section.Clear();
                    }
                }
                else
                {
                    section.Add(line);
                }
            }

            if (section.Count > 0)
            {
                yield return section.ToArray();
            }
        }
    }
}