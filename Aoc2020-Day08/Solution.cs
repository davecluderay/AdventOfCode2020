using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day08
{
    internal class Solution
    {
        public string Title => "Day 8: Handheld Halting";

        public object PartOne()
        {
            var program = ReadProgram();
            return ExecuteProgram(program).acc;
        }
        
        public object PartTwo()
        {
            var program = ReadProgram();

            bool MutateAt(Instruction[] instructions, int index)
            {
                var mutateTo = instructions[index].Code switch
                {
                    "nop" => "jmp",
                    "jmp" => "nop",
                    _ => null
                };

                if (mutateTo != null)
                    instructions[index].Code = mutateTo;

                return mutateTo != null;
            }
     
            for (var index = 0; index < program.Length; index++)
            {
                if (!MutateAt(program, index)) continue;
                
                var result = ExecuteProgram(program);
                if (result.ptr == program.Length)
                    return result.acc;
                
                MutateAt(program, index);
            }

            return null;
        }

        private Instruction[] ReadProgram(string fileName = null)
            => InputFile.ReadAllLines(fileName)
                        .Select(Instruction.Parse)
                        .ToArray();

        private (int ptr, int acc) ExecuteProgram(Instruction[] program)
        {
            var acc = 0;
            var ptr = 0;
            var ptrsExecuted = new HashSet<int>();
            while (ptr < program.Length)
            {
                if (ptrsExecuted.Contains(ptr)) break;
                ptrsExecuted.Add(ptr);

                var instruction = program[ptr];
                switch (instruction.Code)
                {
                    case "acc":
                        acc += instruction.Argument;
                        ptr++;
                        break;
                    case "jmp":
                        ptr += instruction.Argument;
                        break;
                    default:
                        ptr++;
                        break;
                }
            }
            return (ptr, acc);
        }
    }

    public class Instruction
    {
        public static Instruction Parse(string input)
        {
            var parts = input.Split(' ', 2);
            return new Instruction { Code = parts[0], Argument = Convert.ToInt32(parts[1]) };
        }
        public string Code { get; set; }
        public int Argument { get; set; }
    }
}