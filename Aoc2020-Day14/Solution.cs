using Aoc2020_Day14.Computer;

namespace Aoc2020_Day14
{
    internal class Solution
    {
        public string Title => "Day 14: Docking Data";

        public object? PartOne()
            => new ComputerV1().Run();

        public object? PartTwo()
            => new ComputerV2().Run();
    }
}
