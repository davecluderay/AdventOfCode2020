using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day16
{
    internal class Solution
    {
        public string Title => "Day 16: Ticket Translation";

        public object PartOne()
        {
            var rules = GetRules();
            var nearbyTickets = GetNearbyTickets();

            return nearbyTickets.SelectMany(ticket => ticket)
                                .Where(v => rules.All(r => !r.IsValid(v)))
                                .Sum();
        }

        public object PartTwo()
        {
            var rules = GetRules();
            var nearbyTickets = GetNearbyTickets().ToArray();
            var validTickets = nearbyTickets.Where(ticket => ticket.All(value => rules.Any(rule => rule.IsValid(value))))
                                            .ToArray();

            var fieldsInOrder = IdentifyFieldOrder(rules, validTickets);

            var myTicket = GetMyTicket();
            return fieldsInOrder.Where(f => f.name.StartsWith("departure"))
                                .Aggregate(1L,
                                           (result, field) => result * myTicket[field.position],
                                           result => result);
        }

        private static Rule[] GetRules()
            => ReadSection(0, skipFirstLine: false).Select(Rule.Parse)
                                                   .ToArray();

        private static int[] GetMyTicket()
            => ReadSection(1, skipFirstLine: true).Select(l => l.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                                .Select(s => Convert.ToInt32(s))
                                                                .ToArray())
                                                  .Single();

        private static IEnumerable<int[]> GetNearbyTickets()
            => ReadSection(2, skipFirstLine: true).Select(l => l.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                                .Select(s => Convert.ToInt32(s))
                                                                .ToArray())
                                                  .ToArray();

        private static (string name, int position)[] IdentifyFieldOrder(Rule[] rules, IReadOnlyList<int[]> validTickets)
        {
            var fieldCount = validTickets.First().Length;

            var candidatesByFieldName =
                rules.ToDictionary(r => r.FieldName,
                                   _ => new HashSet<int>(Enumerable.Range(0, fieldCount)));

            for (var t = 0; t < validTickets.Count; ++t)
            for (var f = 0; f < fieldCount        ; ++f)
            {
                var value = validTickets[t][f];
                foreach (var rule in rules.Where(r => !r.IsValid(value)))
                    candidatesByFieldName[rule.FieldName].Remove(f);
            }

            var resolved = candidatesByFieldName.Where(x => x.Value.Count == 1)
                                                .ToArray();
            while (resolved.Length < fieldCount)
            {
                foreach (var resolvedEntry in resolved)
                foreach (var entry in candidatesByFieldName.Where(e => e.Key != resolvedEntry.Key))
                    entry.Value.Remove(resolvedEntry.Value.Single());

                resolved = candidatesByFieldName.Where(p => p.Value.Count == 1)
                                                .ToArray();
            }

            var fieldsInOrder = resolved.Select(s => (name: s.Key, position: s.Value.Single()))
                                        .OrderBy(s => s)
                                        .ToArray();
            return fieldsInOrder;
        }

        private static string[] ReadSection(int sectionIndex, bool skipFirstLine)
        {
            var blankLinePattern = new Regex((@"\r?\n\r?\n"), RegexOptions.Compiled);
            var newLinePattern = new Regex((@"\r?\n"), RegexOptions.Compiled);

            var sectionText = blankLinePattern.Split(InputFile.ReadAllText())[sectionIndex];
            var sectionLines = newLinePattern.Split(sectionText).Where(s => !string.IsNullOrEmpty(s));

            if (skipFirstLine) sectionLines = sectionLines.Skip(1);
            return sectionLines.ToArray();
        }
    }
}
