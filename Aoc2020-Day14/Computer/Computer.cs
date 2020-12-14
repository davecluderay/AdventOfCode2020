using System;
using System.Linq;

namespace Aoc2020_Day14.Computer
{
    internal abstract class Computer
    {
        protected abstract void Execute(Instruction instruction, ExecutionState state);

        public long Run(string? fileName = null)
        {
            var state = new ExecutionState();
            foreach (var instruction in ReadProgram(fileName))
            {
                Execute(instruction, state);
            }
            return state.Memory.Values.Sum();
        }

        private Instruction[] ReadProgram(string? fileName = null)
        {
            var lines = InputFile.ReadAllLines(fileName);
            var instructions = lines.Select(l => (Instruction?)StoreInstruction.TryRead(l)
                                                 ?? (Instruction?)MaskInstruction.TryRead(l)
                                                 ?? throw new FormatException($"Unrecognised instruction: {l}"))
                                    .ToArray();
            return instructions;
        }
    }
}