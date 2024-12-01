namespace AdventOfCode.Core;

public class SolvingException : Exception
{
    public SolvingException()
    {
    }

    public SolvingException(string message) : base(message)
    {
    }

    public SolvingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
