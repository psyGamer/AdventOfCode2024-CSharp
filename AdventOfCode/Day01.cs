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

    private readonly List<int> left = [];
    private readonly List<int> right = [];

    public Day01(string[] input)
    {
        foreach (string line in input)
        {
            string[] split = line.Split("   ");
            left.Add(int.Parse(split[0]));
            right.Add(int.Parse(split[1]));
        }
    }

    public ValueTask<string> Solve1()
    {
        left.Sort();
        right.Sort();

        ulong sum = 0;
        foreach ((int l, int r) in left.Zip(right))
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
