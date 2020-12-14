using System;
using System.Text.RegularExpressions;

namespace Aoc2020_Day14.Computer
{
    internal class StoreInstruction : Instruction
    {
        private static readonly Regex Pattern = new Regex(@"^mem\[(?<Address>\d+)\]\s*=\s*(?<Data>\d+)$",
                                                          RegexOptions.Compiled | RegexOptions.Singleline);

        public long Address { get; }
        public long Value { get; }

        private StoreInstruction(long address, long value)
            => (Address, Value) = (address, value);

        public static StoreInstruction? TryRead(string input)
        {
            var match = Pattern.Match(input);
            if (!match.Success) return null;

            return new StoreInstruction(Convert.ToInt64(match.Groups["Address"].Value),
                                        Convert.ToInt64(match.Groups["Data"].Value));
        }
    }
}
