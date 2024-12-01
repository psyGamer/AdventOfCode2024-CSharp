global using TestCase = (string? Part1Result, string? Part2Result, string Input);

namespace AdventOfCode.Core;

public interface IDay
{
    /// <summary>
    /// Number of the current day
    /// </summary>
    public static abstract uint DayNumber { get; }

    /// <summary>
    /// List of test-cases to check against
    /// </summary>
    public static TestCase[] Tests => [];

    /// <summary>
    /// Solver for part 1 of the day
    /// </summary>
    public ValueTask<string> Solve1();

    /// <summary>
    /// Solver for part 1 of the day
    /// </summary>
    public ValueTask<string> Solve2();
}
