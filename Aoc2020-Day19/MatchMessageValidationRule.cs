namespace Aoc2020_Day19
{
    internal class MatchMessageValidationRule : MessageValidationRule
    {
        public char Value { get; }

        public MatchMessageValidationRule(int id, char value, string ruleText) : base (id, ruleText)
            => Value = value;
    }
}
