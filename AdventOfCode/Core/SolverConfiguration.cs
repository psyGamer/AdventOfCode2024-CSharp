using System.Reflection;
using Spectre.Console;

namespace AdventOfCode.Core;

public class SolverConfiguration
{
    /// <summary>
    /// Runs all tests specified in <see cref="IDay.Tests"/>
    /// </summary>
    public bool RunTests { get; set; } = false;

    /// <summary>
    /// Clears previous runs information from the console.
    /// True by default.
    /// </summary>
    public bool ClearConsole { get; set; } = true;

    /// <summary>
    /// Shows a panel at the end of the run with aggregated stats of the solved problems.
    /// True by default when solving multiple problems, false otherwise.
    /// </summary>
    public bool ShowOverallResults { get; set; } = true;

    /// <summary>
    /// Shows the time elapsed during the instantiation of a <see cref="IDay"/>.
    /// This normally reflects the elapsed time while parsing the input data.
    /// </summary>
    public bool ShowConstructorElapsedTime { get; set; } = false;

    /// <summary>
    /// Shows total elapsed time per day. This includes constructor time + part 1 + part 2
    /// </summary>
    public bool ShowTotalElapsedTimePerDay { get; set; } = false;

    /// <summary>
    /// Custom numeric format strings used for elapsed millisecods.
    /// See https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
    /// </summary>
    public string? ElapsedTimeFormatSpecifier { get; set; }

    /// <summary>
    /// Assemblies where the problems are located.
    /// Defaults to the entry assembly: [Assembly.GetEntryAssembly()!]
    /// </summary>
    public List<Assembly> ProblemAssemblies { get; set; } = [Assembly.GetEntryAssembly()!];

    /// <summary>
    /// Represents vertical overflow.
    /// <see href="https://spectreconsole.net/live/live-display"/>
    /// </summary>
    internal VerticalOverflow VerticalOverflow { get; set; } = VerticalOverflow.Ellipsis;

    /// <summary>
    /// Represents vertical overflow cropping.
    /// <see href="https://spectreconsole.net/live/live-display"/>
    /// </summary>
    internal VerticalOverflowCropping VerticalOverflowCropping { get; set; } = VerticalOverflowCropping.Top;
}
