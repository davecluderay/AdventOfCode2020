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
                    var previous = new List<ValidateResult> { new ValidateResult(true, 0) };
                    foreach (var childRule in ruleSet)
                    {
                        previous = previous.SelectMany(
                                               p => Validate(childRule, message, position + p.Consumed)
                                                    .Where(r => r.IsValid)
                                                    .Select(r => new ValidateResult(true, r.Consumed + p.Consumed)))
                                           .ToList();
                    }

                    allValidChildResults.AddRange(previous);
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
