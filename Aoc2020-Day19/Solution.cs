using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day19
{
    internal class Solution
    {
        public string Title => "Day 19: Monster Messages";

        public object PartOne()
        {
            var rules = GetRules();
            var messageValidator = new MessageValidator(rules);
            var messages = GetMessages();
            return messages.Count(m => messageValidator.IsValid(m));
        }

        public object PartTwo()
        {
            var rules = GetRules(s => s.Replace("8: 42", "8: 42 | 42 8")
                                       .Replace("11: 42 31", "11: 42 31 | 42 11 31"));
            var messageValidator = new MessageValidator(rules);
            var messages = GetMessages();
            return messages.Count(m => messageValidator.IsValid(m));
        }

        private static IDictionary<int, MessageValidationRule> GetRules(Func<string, string>? modifier = null)
        {
            var rulesText = ReadSection(0).Select(modifier ?? (s => s))
                                          .ToArray();
            return MessageValidationRule.ReadRules(rulesText);
        }

        private static string[] GetMessages()
        {
            return ReadSection(1).Where(m => !string.IsNullOrEmpty(m))
                                 .ToArray();
        }

        private static string[] ReadSection(int sectionIndex)
        {
            var blankLinePattern = new Regex((@"\r?\n\r?\n"), RegexOptions.Compiled);
            var newLinePattern = new Regex((@"\r?\n"), RegexOptions.Compiled);

            var sectionText = blankLinePattern.Split(InputFile.ReadAllText())[sectionIndex];
            var sectionLines = newLinePattern.Split(sectionText).Where(s => !string.IsNullOrEmpty(s));

            return sectionLines.ToArray();
        }
    }
}
