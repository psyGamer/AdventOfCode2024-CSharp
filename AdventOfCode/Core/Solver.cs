using System.Diagnostics;
using System.Reflection;
using Spectre.Console;

namespace AdventOfCode.Core;

public static class Solver
{
    private static readonly bool IsInteractiveEnvironment = Environment.UserInteractive && !Console.IsOutputRedirected;

    private sealed record ElapsedTime(double Constructor, double Part1, double Part2);
    private static double CalculateElapsedMilliseconds(Stopwatch stopwatch) => 1000 * stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
    private static double CalculateElapsedMilliseconds(long ticks) => 1000 * ticks / (double)Stopwatch.Frequency;

    public static async Task Solve(uint dayNumber, Action<SolverConfiguration>? options = null)
    {
        var config = PopulateConfiguration(options);

        if (IsInteractiveEnvironment && config.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        if (config.RunTests)
        {
            var testTable = GetTestTable();
            await AnsiConsole.Live(testTable)
                .AutoClear(false)
                .Overflow(config.VerticalOverflow)
                .Cropping(config.VerticalOverflowCropping)
                .StartAsync(async ctx =>
                {
                    var problemType = FindProblemTypes(config.ProblemAssemblies).FirstOrDefault(t => dayNumber == (uint)t.GetProperty(nameof(IDay.DayNumber), BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!);
                    if (problemType == null)
                        return;

                    if (!await TestProblem(problemType, testTable, config))
                    {
                        testTable.BorderColor(Color.Red);
                    }
                });
        }

        var solveTable = GetSolveTable();
        await AnsiConsole.Live(solveTable)
            .AutoClear(false)
            .Overflow(config.VerticalOverflow)
            .Cropping(config.VerticalOverflowCropping)
            .StartAsync(async ctx =>
            {
                var problemType = FindProblemTypes(config.ProblemAssemblies).FirstOrDefault(t => dayNumber == (uint)t.GetProperty(nameof(IDay.DayNumber), BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!);
                if (problemType == null)
                {
                    return;
                }

                await SolveProblem(problemType, solveTable, config);
            });
    }

    public static async Task SolveLast(Action<SolverConfiguration>? options = null)
    {
        var config = PopulateConfiguration(options);

        if (IsInteractiveEnvironment && config.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        if (config.RunTests)
        {
            var testTable = GetTestTable();
            await AnsiConsole.Live(testTable)
                .AutoClear(false)
                .Overflow(config.VerticalOverflow)
                .Cropping(config.VerticalOverflowCropping)
                .StartAsync(async ctx =>
                {
                    var problemType = FindProblemTypes(config.ProblemAssemblies).LastOrDefault();
                    if (problemType == null)
                        return;

                    if (!await TestProblem(problemType, testTable, config))
                    {
                        testTable.BorderColor(Color.Red);
                    }
                });
        }

        var solveTable = GetSolveTable();
        await AnsiConsole.Live(solveTable)
            .AutoClear(false)
            .Overflow(config.VerticalOverflow)
            .Cropping(config.VerticalOverflowCropping)
            .StartAsync(async ctx =>
            {
                var problemType = FindProblemTypes(config.ProblemAssemblies).LastOrDefault();
                if (problemType == null)
                {
                    return;
                }

                await SolveProblem(problemType, solveTable, config);
            });
    }

    public static async Task SolveList(IEnumerable<uint> dayNumbers, Action<SolverConfiguration>? options = null)
    {
        var config = PopulateConfiguration(options);

        if (IsInteractiveEnvironment && config.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        if (config.RunTests)
        {
            var testTable = GetTestTable();
            await AnsiConsole.Live(testTable)
                .AutoClear(false)
                .Overflow(config.VerticalOverflow)
                .Cropping(config.VerticalOverflowCropping)
                .StartAsync(async ctx =>
                {
                    var problemTypes = FindProblemTypes(config.ProblemAssemblies)
                        .Where(t => dayNumbers.Contains((uint)t.GetProperty(nameof(IDay.DayNumber), BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!));

                    foreach (var problemType in problemTypes)
                    {
                        if (!await TestProblem(problemType, testTable, config))
                        {
                            testTable.BorderColor(Color.Red);
                        }
                    }
                });
        }

        var totalElapsedTime = new List<ElapsedTime>();

        var solveTable = GetSolveTable();
        await AnsiConsole.Live(solveTable)
            .AutoClear(false)
            .Overflow(config.VerticalOverflow)
            .Cropping(config.VerticalOverflowCropping)
            .StartAsync(async ctx =>
            {
                var problemTypes = FindProblemTypes(config.ProblemAssemblies)
                    .Where(t => dayNumbers.Contains((uint)t.GetProperty(nameof(IDay.DayNumber), BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!));

                foreach (var problemType in problemTypes)
                {
                    if (await SolveProblem(problemType, solveTable, config) is { } elapsedTime)
                    {
                        totalElapsedTime.Add(elapsedTime);
                    }
                }
            });

        RenderOverallResultsPanel(totalElapsedTime, config);
    }

    public static async Task SolveAll(Action<SolverConfiguration>? options = null)
    {
        var config = PopulateConfiguration(options);

        if (IsInteractiveEnvironment && config.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        if (config.RunTests)
        {
            var testTable = GetTestTable();
            await AnsiConsole.Live(testTable)
                .AutoClear(false)
                .Overflow(config.VerticalOverflow)
                .Cropping(config.VerticalOverflowCropping)
                .StartAsync(async ctx =>
                {
                    var problemTypes = FindProblemTypes(config.ProblemAssemblies);

                    foreach (var problemType in problemTypes)
                    {
                        if (!await TestProblem(problemType, testTable, config))
                        {
                            testTable.BorderColor(Color.Red);
                        }
                    }
                });
        }

        var totalElapsedTime = new List<ElapsedTime>();

        var solveTable = GetSolveTable();
        await AnsiConsole.Live(solveTable)
            .AutoClear(false)
            .Overflow(config.VerticalOverflow)
            .Cropping(config.VerticalOverflowCropping)
            .StartAsync(async ctx =>
            {
                var problemTypes = FindProblemTypes(config.ProblemAssemblies);

                foreach (var problemType in problemTypes)
                {
                    if (await SolveProblem(problemType, solveTable, config) is { } elapsedTime)
                    {
                        totalElapsedTime.Add(elapsedTime);
                    }
                }
            });

        RenderOverallResultsPanel(totalElapsedTime, config);
    }

    private static async ValueTask<ElapsedTime?> SolveProblem(Type problemType, Table table, SolverConfiguration config)
    {
        uint problemIndex = (uint)problemType.GetProperty(nameof(IDay.DayNumber), BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!;
        string problemTitle = problemIndex != default
            ? $"Day {problemIndex}"
            : $"{problemType.Name}";

        string filePath = $"{problemIndex:D2}.txt";

        var sw = new Stopwatch();

        long accumTicks = 0;

        IDay day = null!;
        try {
            if (problemType.GetConstructor([typeof(string)]) is { } dayTextConstructor)
            {
                for (int i = 0; i < config.MeasurementRepeats; i++)
                {
                    sw.Restart();
                    day = (IDay)dayTextConstructor.Invoke([await File.ReadAllTextAsync(filePath)]);
                    sw.Stop();

                    accumTicks += sw.ElapsedTicks;
                }
            }
            else if (problemType.GetConstructor([typeof(string[])]) is { } dayLinesConstructor)
            {
                for (int i = 0; i < config.MeasurementRepeats; i++)
                {
                    sw.Restart();
                    day = (IDay)dayLinesConstructor.Invoke([await File.ReadAllLinesAsync(filePath)]);
                    sw.Stop();

                    accumTicks += sw.ElapsedTicks;
                }
            }
            else
            {
                RenderEmptySolveRow(problemType, "No suitable constructor found.", table, 0, config);
                return null;
            }
        }
        catch (Exception ex)
        {
            RenderEmptySolveRow(problemType, ex.InnerException?.ToString() ?? ex.ToString(), table, 0, config);
            return null;
        }

        double elapsedMillisecondsCtor = CalculateElapsedMilliseconds(accumTicks) / config.MeasurementRepeats;
        if (config.ShowConstructorElapsedTime)
        {
            RenderSolveRow(table, problemTitle, $"{day.GetType().Name}()", "-----------", elapsedMillisecondsCtor, config);
        }

        (string solution1, double elapsedMillisecondsPart1) = await SolvePart(day.Solve1, config);
        RenderSolveRow(table, problemTitle, "Part 1", solution1, elapsedMillisecondsPart1, config);

        (string solution2, double elapsedMillisecondsPart2) = await SolvePart(day.Solve2, config);
        RenderSolveRow(table, problemTitle, "Part 2", solution2, elapsedMillisecondsPart2, config);

        if (config.ShowTotalElapsedTimePerDay)
        {
            RenderSolveRow(table, problemTitle, "[bold]Total[/]", "-----------", elapsedMillisecondsCtor + elapsedMillisecondsPart1 + elapsedMillisecondsPart2, config);
        }

        table.AddEmptyRow();

        return new ElapsedTime(elapsedMillisecondsCtor, elapsedMillisecondsPart1, elapsedMillisecondsPart2);
    }
    private static async ValueTask<bool> TestProblem(Type problemType, Table table, SolverConfiguration config)
    {
        uint problemIndex = (uint)problemType.GetProperty(nameof(IDay.DayNumber), BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!;
        string problemTitle = problemIndex != default
            ? $"Day {problemIndex}"
            : $"{problemType.Name}";

        bool success = true;

        var tests = (TestCase[]?)problemType.GetProperty(nameof(IDay.Tests), BindingFlags.Public | BindingFlags.Static)?.GetValue(null) ?? [];
        foreach (var test in tests)
        {
            IDay day;
            try {
                if (problemType.GetConstructor([typeof(string)]) is { } dayTextConstructor)
                {
                    day = (IDay)dayTextConstructor.Invoke([test.Input]);
                }
                else if (problemType.GetConstructor([typeof(string[])]) is { } dayLinesConstructor)
                {
                    day = (IDay)dayLinesConstructor.Invoke([test.Input.Split("\n").ToArray()]);
                }
                else
                {
                    RenderEmptyTestRow(problemType, "No suitable constructor found.", table, 0, config);
                    success = false;
                    continue;
                }
            }
            catch (Exception ex)
            {
                RenderEmptyTestRow(problemType, ex.InnerException?.ToString() ?? ex.ToString(), table, 0, config);
                success = false;
                continue;
            }

            if (test.Part1Result != null)
            {
                (string solution1, _) = await SolvePart(day.Solve1, config);
                bool match = string.Equals(solution1, test.Part1Result);
                RenderTestRow(table, problemTitle, "Part 1", solution1, test.Part1Result, match);

                success &= match;
            }

            if (test.Part2Result != null)
            {
                (string solution2, _) = await SolvePart(day.Solve2, config);
                bool match = string.Equals(solution2, test.Part2Result);
                RenderTestRow(table, problemTitle, "Part 2", solution2, test.Part2Result, match);

                success &= match;
            }

            table.AddEmptyRow();

        }

        return success;
    }

    private static async ValueTask<(string solution, double elapsedTime)> SolvePart(Func<ValueTask<string>> solve, SolverConfiguration config)
    {
        Stopwatch stopwatch = new();
        string solution = null!;

        try
        {
            for (int i = 0; i < config.MeasurementRepeats; i++)
            {
                stopwatch.Restart();
                solution = await solve();
                stopwatch.Stop();
            }
        }
        catch (NotImplementedException)
        {
            solution = "[[Not implemented]]";
            stopwatch.Stop();
        }
        catch (Exception e)
        {
            solution = e.Message + Environment.NewLine + e.StackTrace;
            stopwatch.Stop();
        }

        double elapsedMilliseconds = CalculateElapsedMilliseconds(stopwatch) / config.MeasurementRepeats;
        return (solution, elapsedMilliseconds);
    }

    private static string FormatTime(double elapsedMilliseconds, SolverConfiguration configuration, bool useColor = true)
    {
        var customFormatSpecifier = configuration?.ElapsedTimeFormatSpecifier;

        var message = customFormatSpecifier is null
            ? elapsedMilliseconds switch
            {
                < 1 => $"{elapsedMilliseconds:F} ms",
                < 1_000 => $"{Math.Round(elapsedMilliseconds)} ms",
                < 60_000 => $"{0.001 * elapsedMilliseconds:F} s",
                _ => $"{Math.Floor(elapsedMilliseconds / 60_000)} min {Math.Round(0.001 * (elapsedMilliseconds % 60_000))} s",
            }
            : elapsedMilliseconds switch
            {
                < 1 => $"{elapsedMilliseconds.ToString(customFormatSpecifier)} ms",
                < 1_000 => $"{elapsedMilliseconds.ToString(customFormatSpecifier)} ms",
                < 60_000 => $"{(0.001 * elapsedMilliseconds).ToString(customFormatSpecifier)} s",
                _ => $"{elapsedMilliseconds / 60_000} min {(0.001 * (elapsedMilliseconds % 60_000)).ToString(customFormatSpecifier)} s",
            };

        if (useColor)
        {
            var color = elapsedMilliseconds switch
            {
                < 1 => Color.Blue,
                < 10 => Color.Green1,
                < 100 => Color.Lime,
                < 500 => Color.GreenYellow,
                < 1_000 => Color.Yellow1,
                < 10_000 => Color.OrangeRed1,
                _ => Color.Red1
            };

            return $"[{color}]{message}[/]";
        }
        else
        {
            return message;
        }
    }

    private static Table GetTestTable()
    {
        return new Table()
            .Title("Test")
            .AddColumns(
                "[bold]Day[/]",
                "[bold]Part[/]",
                "[bold]Expected Solution[/]",
                "[bold]Actual Solution[/]",
                "[bold]Success[/]")
            .RoundedBorder()
            .BorderColor(Color.Green);
    }
    private static Table GetSolveTable()
    {
        return new Table()
            .Title("Solve")
            .AddColumns(
                "[bold]Day[/]",
                "[bold]Part[/]",
                "[bold]Solution[/]",
                "[bold]Elapsed time[/]")
            .RoundedBorder()
            .BorderColor(Color.Grey);
    }

    private static void RenderSolveRow(Table table, string problemTitle, string part, string solution, double elapsedMilliseconds, SolverConfiguration configuration)
    {
        string formattedTime = FormatTime(elapsedMilliseconds, configuration);
        table.AddRow(problemTitle, part, solution.EscapeMarkup(), formattedTime);
    }
    private static ElapsedTime RenderEmptySolveRow(Type problemType, string? exceptionString, Table table, double constructorElapsedTime, SolverConfiguration config)
    {
        string problemTitle = problemType.Name;

        RenderSolveRow(table, problemTitle, $"{problemTitle}()", exceptionString ?? "Unhandled exception during constructor", constructorElapsedTime, config);

        const double elapsedMillisecondsPart1 = 0;
        const double elapsedMillisecondsPart2 = 0;
        RenderSolveRow(table, problemTitle, "Part 1", "-----------", elapsedMillisecondsPart1, config);
        RenderSolveRow(table, problemTitle, "Part 2", "-----------", elapsedMillisecondsPart2, config);

        if (config.ShowTotalElapsedTimePerDay)
        {
            RenderSolveRow(table, problemTitle, "[bold]Total[/]", "-----------", constructorElapsedTime + elapsedMillisecondsPart1 + elapsedMillisecondsPart2, config);
        }

        table.AddEmptyRow();

        return new ElapsedTime(constructorElapsedTime, elapsedMillisecondsPart1, elapsedMillisecondsPart2);
    }
    private static void RenderOverallResultsPanel(List<ElapsedTime> totalElapsedTime, SolverConfiguration config)
    {
        if (config.ShowOverallResults != true || totalElapsedTime.Count <= 1)
        {
            return;
        }

        double totalConstructors = totalElapsedTime.Sum(t => t.Constructor);
        double totalPart1 = totalElapsedTime.Sum(t => t.Part1);
        double totalPart2 = totalElapsedTime.Sum(t => t.Part2);
        double total = totalPart1 + totalPart2 + (config.ShowConstructorElapsedTime ? totalConstructors : 0);

        var grid = new Grid()
            .AddColumn(new GridColumn().NoWrap().PadRight(4))
            .AddColumn()
            .AddRow()
            .AddRow($"[bold]Total ({totalElapsedTime.Count} days[/])", FormatTime(total, config, useColor: false));

        if (config.ShowConstructorElapsedTime)
        {
            grid.AddRow("Total constructors", FormatTime(totalConstructors, config, useColor: false));
        }

        grid
            .AddRow("Total parts 1", FormatTime(totalPart1, config, useColor: false))
            .AddRow("Total parts 2", FormatTime(totalPart2, config, useColor: false))
            .AddRow()
            .AddRow("[bold]Mean (per day)[/]", FormatTime(total / totalElapsedTime.Count, config));

        if (config.ShowConstructorElapsedTime)
        {
            grid.AddRow("Mean constructors", FormatTime(totalElapsedTime.Average(t => t.Constructor), config));
        }

        grid
            .AddRow("Mean parts 1", FormatTime(totalElapsedTime.Average(t => t.Part1), config))
            .AddRow("Mean parts 2", FormatTime(totalElapsedTime.Average(t => t.Part2), config));

        AnsiConsole.Write(
            new Panel(grid)
                .Header("[b] Overall results [/]", Justify.Center));
    }

    private static void RenderTestRow(Table table, string problemTitle, string part, string actual, string expected, bool success)
    {
        table.AddRow(problemTitle, part, actual.EscapeMarkup(), expected.EscapeMarkup(), success ? "[bold green]Pass[/]" : "[bold red]Fail[/]");
    }
    private static void RenderEmptyTestRow(Type problemType, string? exceptionString, Table table, double constructorElapsedTime, SolverConfiguration config)
    {
        string problemTitle = problemType.Name;

        RenderTestRow(table, problemTitle, $"{problemTitle}()", exceptionString ?? "Unhandled exception during constructor", "", false);
        RenderTestRow(table, problemTitle, "Part 1", "-----------", "-----------", false);
        RenderTestRow(table, problemTitle, "Part 2", "-----------", "-----------", false);

        table.AddEmptyRow();
    }

    private static IEnumerable<Type> FindProblemTypes(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IDay).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .OrderBy(t => (uint)t.GetProperty(nameof(IDay.DayNumber), BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!);
    }
    private static SolverConfiguration PopulateConfiguration(Action<SolverConfiguration>? options)
    {
        var configuration = new SolverConfiguration();
        options?.Invoke(configuration);

        return configuration;
    }
}
