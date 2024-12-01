namespace AdventOfCode;

public class Day01 : IDay
{
    public static uint DayNumber => 1;

    public static TestCase[] Tests => [
        ("11", "31",
        """
        3   4
        4   3
        2   5
        1   3
        3   9
        3   3
        """)
    ];

    private readonly uint[] left;
    private readonly uint[] right;

    public Day01(string[] input)
    {
        Span<Range> splits = [default, default];

        left = new uint[input.Length];
        right = new uint[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            var line = input[0].AsSpan();

            if (line.Length == 0) continue;
            line.Split(splits, "   ");

            left[i] = uint.Parse(line[splits[0]]);
            right[i] = uint.Parse(line[splits[1]]);
        }
    }

    public ValueTask<string> Solve1()
    {
        ulong sum = 0;
        foreach ((uint l, uint r) in left.Order().Zip(right.Order()))
        {
            sum += (ulong)Math.Abs(l - r);
        }

        return ValueTask.FromResult(sum.ToString());
    }
    public ValueTask<string> Solve2()
    {
        ulong sum = 0;
        foreach (int leftVal in left)
        {
            sum += (ulong)(leftVal * right.Count(v => v == leftVal));
        }

        return ValueTask.FromResult(sum.ToString());
    }
}
