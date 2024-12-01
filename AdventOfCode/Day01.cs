namespace AdventOfCode;

public class Day01(string input) : IDay
{
    public static uint DayNumber => 1;

    public static TestCase[] Tests => [
        (null, $"Solution to Day {DayNumber}, part 2: abc",
        """abc""")
    ];

    public ValueTask<string> Solve1() => new($"Solution to Day {DayNumber}, part 1: {input}");
    public ValueTask<string> Solve2() => new($"Solution to Day {DayNumber}, part 2: {input}");
}
