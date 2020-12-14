using System.Linq;

namespace Aoc2020_Day14.Computer
{
    internal sealed class ComputerV1 : Computer
    {
        protected override void Execute(Instruction instruction, ExecutionState state)
        {
            switch (instruction)
            {
                case StoreInstruction store:
                    var orMask = state.Mask.Select((c, i) => (c, bit: 35 - i))
                                           .Where(x => x.c == '1')
                                           .Sum(x => 1L << x.bit);
                    var andMask = ~ state.Mask.Select((c, i) => (c, bit: 35 - i))
                                              .Where(x => x.c == '0')
                                              .Sum(x => 1L << x.bit);
                    state.Memory[store.Address] = store.Value & andMask | orMask;
                    break;

                case MaskInstruction mask:
                    state.Mask = mask.Mask;
                    break;
            }
        }
    }
}
