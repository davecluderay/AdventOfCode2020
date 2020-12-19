using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day19
{
    internal class MessageValidator
    {
        private readonly IDictionary<int, MessageValidationRule> _rules;

        public MessageValidator(IDictionary<int, MessageValidationRule> rules)
        {
            _rules = rules;
        }

        public bool IsValid(string message)
        {
            var results = Validate(_rules[0], message, 0);
            return results.Any(result => result.IsValid && result.Consumed == message.Length);
        }

        private ValidateResult[] Validate(MessageValidationRule rule, string message, int position)
        {
            if (position >= message.Length)
            {
                return Array.Empty<ValidateResult>();
            }

            if (rule is MatchMessageValidationRule matchRule)
            {
                var isValid = matchRule.Value == message[position];
                return isValid
                           ? new[] { new ValidateResult(true, 1) }
                           : Array.Empty<ValidateResult>();
            }

            if (rule is CompositeMessageValidationRule compositeRule)
            {
                var allValidChildResults = new List<ValidateResult>();
                foreach (var ruleSet in compositeRule.ChildRuleSets)
                {
                    var resultsByChildIndex = Enumerable.Range(0, ruleSet.Count).ToDictionary(i => i, i => new List<ValidateResult>());
                    for (var i = 0; i < ruleSet.Count; i++)
                    {
                        var childRule = ruleSet[i];
                        var previousChildResults = i > 0 ? resultsByChildIndex[i - 1] : new List<ValidateResult> { new ValidateResult(true, 0) };
                        resultsByChildIndex[i].AddRange(previousChildResults.SelectMany(cr => Validate(childRule, message, position + cr.Consumed)
                                                                      .Where(r => r.IsValid)
                                                                      .Select(r => new ValidateResult(true, r.Consumed + cr.Consumed))));
                    }

                    allValidChildResults.AddRange(resultsByChildIndex[ruleSet.Count - 1]);
                }

                return allValidChildResults.ToArray();
            }

            throw new ArgumentException($"Unrecognised rule type: {rule.GetType().Name}");
        }

        private class ValidateResult
        {
            public ValidateResult(bool isValid, int consumed)
                => (IsValid, Consumed) = (isValid, consumed);
            public bool IsValid { get; }
            public int Consumed { get; }
        }
    }
}
