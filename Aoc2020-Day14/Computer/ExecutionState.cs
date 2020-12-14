using System.Collections.Generic;

namespace Aoc2020_Day14.Computer
{
    internal class ExecutionState
    {
        public string Mask { get; set; } = "111111111111111111111111111111111111";
        public IDictionary<long, long> Memory { get; } = new Dictionary<long, long>();
    }
}
