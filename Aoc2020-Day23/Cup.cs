namespace Aoc2020_Day23
{
    internal class Cup
    {
        public Cup(long label)
        {
            Label = label;
            Next = this;
        }

        public long Label { get; }
        public Cup Next { get; set; }

        public override string ToString()
            => $"Cup {Label}";
    }
}
