using System.Collections.Generic;

namespace Aoc2020_Day19
{
    internal class CompositeMessageValidationRule : MessageValidationRule
    {
        public IList<IList<MessageValidationRule>> ChildRuleSets { get; }

        public CompositeMessageValidationRule(int id, string ruleText) : base(id, ruleText)
        {
            ChildRuleSets = new List<IList<MessageValidationRule>>();
        }
    }
}
