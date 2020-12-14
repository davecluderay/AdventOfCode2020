using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day14.Computer
{
    internal class ComputerV2 : Computer
    {
        protected override void Execute(Instruction instruction, ExecutionState state)
        {
            switch (instruction)
            {
                case StoreInstruction store:
                    foreach (var address in DecodeAddress(store.Address, state.Mask))
                        state.Memory[address] = store.Value;
                    break;

                case MaskInstruction mask:
                    state.Mask = mask.Mask;
                    break;
            }
        }

        private IEnumerable<long> DecodeAddress(long address, string mask)
        {
            var orMask = mask.Select((c, i) => (c, bit: 35 - i))
                             .Where(x => x.c != '0')
                             .Sum(x => 1L << x.bit);

            // The OR mask sets all floating bits to one.
            var floatingBits = mask.Select((c, i) => (c, bit: 35 - i))
                                   .Where(x => x.c == 'X')
                                   .Select(x => x.bit)
                                   .ToArray();
            if (!floatingBits.Any()) yield
                return address | orMask;

            // For each combination of floating bits, create an AND mask to flip them into the right state.
            foreach (var n in Enumerable.Range(0, 1 << floatingBits.Length).Select(n => (long)n))
            {
                var andMask = ~ Enumerable.Range(0, floatingBits.Length)
                                          .Sum(x => ((n >> (floatingBits.Length - x - 1)) % 2) << floatingBits[x]);
                yield return (address | orMask) & andMask;
            }
        }
    }
}
