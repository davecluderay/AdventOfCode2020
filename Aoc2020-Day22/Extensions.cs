using System.Collections.Generic;

namespace Aoc2020_Day22
{
    internal static class Extensions
    {
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
