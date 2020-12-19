using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day19
{
    internal abstract class MessageValidationRule
    {
        public int Id { get; }
        public string RuleText { get; }

        protected MessageValidationRule(int id, string ruleText)
        {
            (Id, RuleText) = (id, ruleText);
        }

        public static IDictionary<int, MessageValidationRule> ReadRules(string[] rulesText)
        {
            Regex matchRulePattern = new Regex(@"^(?<Id>\d+):\s*[""](?<Text>[^""])[""]$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
            Regex compositeRulePattern =  new Regex(@"^(?<Id>\d+):\s*(?<RuleIdSetsText>[\d |]+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

            var rules = new Dictionary<int, MessageValidationRule>();
            var ruleIdSets = new Dictionary<int, int[][]>();
            foreach (var text in rulesText)
            {
                var match = matchRulePattern.Match(text);
                if (match.Success)
                {
                    var id = Convert.ToInt32(match.Groups["Id"].Value);
                    var value = match.Groups["Text"].Value.Single();
                    rules[id] = new MatchMessageValidationRule(id, value, text);
                    continue;
                }

                match = compositeRulePattern.Match(text);
                if (match.Success)
                {
                    var id = Convert.ToInt32(match.Groups["Id"].Value);
                    rules[id] = new CompositeMessageValidationRule(id, text);

                    ruleIdSets[id] = match.Groups["RuleIdSetsText"].Value
                                          .Split('|')
                                          .Select(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(i => Convert.ToInt32(i))
                                                        .ToArray())
                                          .ToArray();
                    continue;
                }

                throw new FormatException($"Bad rule: {text}");
            }

            foreach (var entry in ruleIdSets)
            {
                var rule = (CompositeMessageValidationRule)rules[entry.Key];
                foreach (var set in entry.Value)
                {
                    rule.ChildRuleSets.Add(set.Select(id => rules[id]).ToList());
                }
            }

            return rules;
        }

        public override string ToString() => $"{{ {RuleText} }}";
    }
}
