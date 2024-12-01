global using AdventOfCode.Core;

#if DEBUG
await Solver.SolveLast(opt =>
{
    opt.ClearConsole = false;
    opt.RunTests = true;
    opt.ShowTotalElapsedTimePerDay = true;
    opt.ShowConstructorElapsedTime = true;
    opt.ShowOverallResults = false;
});
#else
await Solver.SolveAll(opt =>
{
    opt.ClearConsole = false;
    opt.RunTests = false;
    opt.ShowTotalElapsedTimePerDay = true;
    opt.ShowConstructorElapsedTime = true;
    opt.ShowOverallResults = true;
});
#endif
