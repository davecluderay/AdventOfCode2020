using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day07
{
    internal class Solution
    {
        public string Title => "Day 7: Handy Haversacks";

        public object PartOne()
        {
            var rulesByChildColour = ReadRules().ToLookup(r => r.ChildColour, r => r);

            var possibleParentColours = new HashSet<string>();
            var stack = new Stack<string>();
            stack.Push("shiny gold");

            while (stack.Count > 0)
            {
                var childColour = stack.Pop();
                foreach (var rule in rulesByChildColour[childColour])
                {
                    possibleParentColours.Add(rule.ParentColour);
                    stack.Push(rule.ParentColour);
                }
            }

            return possibleParentColours.Count;
        }
        
        public object PartTwo()
        {
            var rulesByParentColour = ReadRules().ToLookup(r => r.ParentColour, r => r);

            var total = 0;
            var stack = new Stack<(string Colour, int Quantity)>();
            stack.Push(("shiny gold", 1));

            while (stack.Count > 0)
            {
                var parent = stack.Pop();
                total += parent.Quantity;
                foreach (var rule in rulesByParentColour[parent.Colour])
                {
                    stack.Push((rule.ChildColour, rule.ChildQuantity * parent.Quantity));
                }
            }

            return total - 1;
        }

        private static (string ParentColour, string ChildColour, int ChildQuantity)[] ReadRules(string fileName = null)
        {
            var pattern = new Regex(@"((?<quantity>\d+)\s+)?(?<colour>(\S+)(\s\S+)?) bags?", RegexOptions.Compiled);

            IEnumerable<(string, string, int)> ParseRules(string line)
            {
                var matches = pattern.Matches(line);
                var colour = matches.First().Groups["colour"].Value;
                var contents = matches
                    .Skip(1)
                    .Where(m => m.Groups["quantity"].Success)
                    .Select(m => (colour: m.Groups["colour"].Value,
                                  quantity: Convert.ToInt32(m.Groups["quantity"].Value)));
                return contents.Select(c => (colour, c.colour, c.quantity));
            }

            return InputFile.ReadAllLines(fileName)
                            .SelectMany(ParseRules)
                            .ToArray();
        }
    }
}