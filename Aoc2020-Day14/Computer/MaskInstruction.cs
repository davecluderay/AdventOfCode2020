using System.Text.RegularExpressions;

namespace Aoc2020_Day14.Computer
{
    internal class MaskInstruction : Instruction
    {
        private static readonly Regex Pattern = new Regex(@"^mask\s*=\s*(?<Mask>[01X]{36})$",
                                                          RegexOptions.Compiled | RegexOptions.Singleline);
        public string Mask { get; }

        private MaskInstruction(string mask)
            => Mask = mask;

        public static MaskInstruction? TryRead(string input)
        {
            var match = Pattern.Match(input);
            if (!match.Success) return null;

            var mask = match.Groups["Mask"].Value;
            return new MaskInstruction(mask);
        }
    }
}
